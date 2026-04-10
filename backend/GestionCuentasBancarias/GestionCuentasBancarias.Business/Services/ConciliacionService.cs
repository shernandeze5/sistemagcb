using CsvHelper;
using CsvHelper.Configuration;
using GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System.Globalization;

namespace GestionCuentasBancarias.Business.Services
{
    public class ConciliacionService : IConciliacionService
    {
        private readonly IConciliacionRepository repository;

        public ConciliacionService(IConciliacionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> Procesar(ProcesarConciliacionDTO dto)
        {
            ValidarEntrada(dto);

            bool cuentaExiste = await repository.ExisteCuentaActiva(dto.CUB_Cuenta);
            if (!cuentaExiste)
                throw new InvalidOperationException("La cuenta bancaria no existe o está inactiva.");

            bool conciliacionExiste = await repository.ExisteConciliacionPeriodo(dto.CUB_Cuenta, dto.CON_Periodo);
            if (conciliacionExiste)
                throw new InvalidOperationException("Ya existe una conciliación para esa cuenta y ese período.");

            var rango = ObtenerRangoPeriodo(dto.CON_Periodo);

            var temporales = await LeerCsv(dto.Archivo!);
            if (!temporales.Any())
                throw new InvalidOperationException("El archivo no contiene movimientos válidos.");

            var movimientosSistema = (await repository.ObtenerMovimientosSistema(
                dto.CUB_Cuenta,
                rango.FechaInicio,
                rango.FechaFin)).ToList();

            int estadoConciliadoId = await repository.ObtenerEstadoDetalleId("Conciliado");
            int estadoPendienteLibrosId = await repository.ObtenerEstadoDetalleId("Pendiente en libros");
            int estadoPendienteBancoId = await repository.ObtenerEstadoDetalleId("Pendiente en banco");
            int estadoEnTransitoId = await repository.ObtenerEstadoDetalleId("En transito");
            int estadoDifMontoId = await repository.ObtenerEstadoDetalleId("Diferencia de monto");
            int estadoDifFechaId = await repository.ObtenerEstadoDetalleId("Diferencia de fecha");
            int estadoCabeceraDifId = await repository.ObtenerEstadoConciliacionId("Con diferencias");
            int estadoCabeceraOkId = await repository.ObtenerEstadoConciliacionId("Conciliada");

            var detalles = new List<CreateDetalleConciliacionDTO>();
            var usadosSistema = new HashSet<int>();
            var usadosTemp = new HashSet<int>();

            foreach (var temp in temporales.Where(x => !string.IsNullOrWhiteSpace(x.MTE_Referencia)))
            {
                string refTemp = Normalizar(temp.MTE_Referencia);
                decimal montoTemp = ObtenerMontoFirmadoTemporal(temp);

                var candidatoRef = movimientosSistema
                    .Where(m => !usadosSistema.Contains(m.MOV_Movimiento))
                    .FirstOrDefault(m => Normalizar(m.MOV_Numero_Referencia) == refTemp);

                if (candidatoRef == null)
                    continue;

                usadosSistema.Add(candidatoRef.MOV_Movimiento);
                usadosTemp.Add(temp.TempKey);

                decimal montoSistema = ObtenerMontoFirmadoSistema(candidatoRef);
                decimal diferenciaMonto = Math.Abs(montoSistema - montoTemp);
                int diferenciaDias = Math.Abs((candidatoRef.MOV_Fecha.Date - temp.MTE_Fecha.Date).Days);

                int estadoDetalle;

                if (diferenciaMonto > 0.01m)
                    estadoDetalle = estadoDifMontoId;
                else if (diferenciaDias > 1)
                    estadoDetalle = estadoDifFechaId;
                else
                    estadoDetalle = estadoConciliadoId;

                detalles.Add(new CreateDetalleConciliacionDTO
                {
                    MOV_Movimiento = candidatoRef.MOV_Movimiento,
                    TempKey = temp.TempKey,
                    EDC_Estado_Detalle_Conciliacion = estadoDetalle
                });
            }

            foreach (var temp in temporales.Where(x => !usadosTemp.Contains(x.TempKey)))
            {
                decimal montoTemp = ObtenerMontoFirmadoTemporal(temp);

                var candidato = movimientosSistema
                    .Where(m => !usadosSistema.Contains(m.MOV_Movimiento))
                    .FirstOrDefault(m =>
                        Math.Abs(ObtenerMontoFirmadoSistema(m) - montoTemp) <= 0.01m &&
                        Math.Abs((m.MOV_Fecha.Date - temp.MTE_Fecha.Date).Days) <= 1);

                if (candidato == null)
                    continue;

                usadosSistema.Add(candidato.MOV_Movimiento);
                usadosTemp.Add(temp.TempKey);

                detalles.Add(new CreateDetalleConciliacionDTO
                {
                    MOV_Movimiento = candidato.MOV_Movimiento,
                    TempKey = temp.TempKey,
                    EDC_Estado_Detalle_Conciliacion = estadoConciliadoId
                });
            }

            foreach (var temp in temporales.Where(x => !usadosTemp.Contains(x.TempKey)))
            {
                detalles.Add(new CreateDetalleConciliacionDTO
                {
                    MOV_Movimiento = null,
                    TempKey = temp.TempKey,
                    EDC_Estado_Detalle_Conciliacion = estadoPendienteLibrosId
                });
            }

            foreach (var mov in movimientosSistema.Where(x => !usadosSistema.Contains(x.MOV_Movimiento)))
            {
                int estadoDetalle = EsEnTransito(mov, rango.FechaFin)
                    ? estadoEnTransitoId
                    : estadoPendienteBancoId;

                detalles.Add(new CreateDetalleConciliacionDTO
                {
                    MOV_Movimiento = mov.MOV_Movimiento,
                    TempKey = null,
                    EDC_Estado_Detalle_Conciliacion = estadoDetalle
                });
            }

            decimal saldoBanco = temporales.OrderBy(x => x.MTE_Fecha).ThenBy(x => x.TempKey).Last().MTE_Saldo;
            decimal saldoLibros = await repository.ObtenerSaldoLibrosAlCorte(dto.CUB_Cuenta, rango.FechaFin);
            decimal diferencia = saldoBanco - saldoLibros;

            int estadoCabecera = Math.Abs(diferencia) <= 0.01m &&
                                 detalles.All(d => d.EDC_Estado_Detalle_Conciliacion == estadoConciliadoId)
                ? estadoCabeceraOkId
                : estadoCabeceraDifId;

            var guardar = new GuardarConciliacionDTO
            {
                CUB_Cuenta = dto.CUB_Cuenta,
                CON_Periodo = dto.CON_Periodo,
                ARC_Nombre_Archivo = dto.Archivo!.FileName,
                CON_Saldo_Banco = saldoBanco,
                CON_Saldo_Libros = saldoLibros,
                CON_Diferencia = diferencia,
                ECO_Estado_Conciliacion = estadoCabecera,
                Temporales = temporales,
                Detalles = detalles
            };

            return await repository.GuardarProcesoConciliacion(guardar);
        }

        public async Task<ConciliacionResponseDTO?> ObtenerPorId(int conciliacionId)
        {
            if (conciliacionId <= 0)
                throw new InvalidOperationException("El id de conciliación es inválido.");

            var cabecera = await repository.ObtenerPorId(conciliacionId);
            if (cabecera == null)
                return null;

            var detalle = (await repository.ObtenerDetalle(conciliacionId)).ToList();
            EnriquecerDocumentoConciliacion(cabecera, detalle);

            return cabecera;
        }

        public async Task<IEnumerable<DetalleConciliacionResponseDTO>> ObtenerDetalle(int conciliacionId)
        {
            if (conciliacionId <= 0)
                throw new InvalidOperationException("El id de conciliación es inválido.");

            return await repository.ObtenerDetalle(conciliacionId);
        }

        public async Task<IEnumerable<ConciliacionResponseDTO>> ObtenerPorCuenta(int cuentaId)
        {
            if (cuentaId <= 0)
                throw new InvalidOperationException("La cuenta es obligatoria.");

            var lista = (await repository.ObtenerPorCuenta(cuentaId)).ToList();

            foreach (var item in lista)
            {
                var detalle = (await repository.ObtenerDetalle(item.CON_Conciliacion)).ToList();
                EnriquecerDocumentoConciliacion(item, detalle);
            }

            return lista;
        }

        public async Task<IEnumerable<ConciliacionResponseDTO>> ObtenerTodas()
        {
            var lista = (await repository.ObtenerTodas()).ToList();

            foreach (var item in lista)
            {
                var detalle = (await repository.ObtenerDetalle(item.CON_Conciliacion)).ToList();
                EnriquecerDocumentoConciliacion(item, detalle);
            }

            return lista;
        }

        public async Task RegistrarEnLibros(int detalleId)
        {
            if (detalleId <= 0)
                throw new InvalidOperationException("El detalle es inválido.");

            var ctx = await repository.ObtenerDetalleContexto(detalleId);
            if (ctx == null)
                throw new InvalidOperationException("Detalle no encontrado.");

            if (!ctx.MTE_Movimiento_Temporal.HasValue)
                throw new InvalidOperationException("Ese detalle no tiene movimiento bancario para registrar.");

            string estadoActual = (ctx.EDC_Descripcion ?? string.Empty).Trim().ToUpperInvariant();
            if (estadoActual == "CONCILIADO" || estadoActual == "AJUSTADO")
                throw new InvalidOperationException("Ese detalle ya está resuelto.");

            decimal monto = ObtenerMontoTemporalAbsoluto(ctx);
            if (monto <= 0)
                throw new InvalidOperationException("No se pudo determinar el monto del movimiento bancario.");

            string referencia = string.IsNullOrWhiteSpace(ctx.MTE_Referencia)
                ? $"CONC-{ctx.CON_Conciliacion}-DET-{ctx.DCO_Detalle_Conciliacion}"
                : ctx.MTE_Referencia!.Trim();

            bool existe = await repository.ExisteMovimientoCuentaPorReferenciaMonto(ctx.CUB_Cuenta, referencia, monto);
            if (existe)
                throw new InvalidOperationException("Ya existe un movimiento en libros con esa referencia y monto.");

            string tipoDescripcion = (ctx.MTE_Credito ?? 0m) > 0 ? "Ingreso" : "Egreso";
            int tipoId = await repository.ObtenerTipoMovimientoId(tipoDescripcion);

            string medio = InferirMedioMovimiento(ctx.MTE_Descripcion, tipoDescripcion);
            int medioId = await repository.ObtenerMedioMovimientoId(medio);

            int estadoMovimientoId = await repository.ObtenerEstadoMovimientoId("Activo");

            await repository.CrearMovimientoDesdeBanco(
                ctx.CUB_Cuenta,
                ctx.MTE_Fecha ?? DateTime.Now,
                referencia,
                string.IsNullOrWhiteSpace(ctx.MTE_Descripcion) ? "Movimiento registrado desde conciliación" : ctx.MTE_Descripcion!,
                monto,
                tipoId,
                medioId,
                estadoMovimientoId);

            int estadoAjustadoId = await repository.ObtenerEstadoDetalleId("Ajustado");
            await repository.ActualizarEstadoDetalle(detalleId, estadoAjustadoId);

            await RecalcularEstado(ctx.CON_Conciliacion);
        }

        public async Task MarcarEnTransito(int detalleId)
        {
            if (detalleId <= 0)
                throw new InvalidOperationException("El detalle es inválido.");

            var ctx = await repository.ObtenerDetalleContexto(detalleId);
            if (ctx == null)
                throw new InvalidOperationException("Detalle no encontrado.");

            int estadoEnTransitoId = await repository.ObtenerEstadoDetalleId("En transito");
            await repository.ActualizarEstadoDetalle(detalleId, estadoEnTransitoId);

            await RecalcularEstado(ctx.CON_Conciliacion);
        }

        public async Task AceptarManual(int detalleId)
        {
            if (detalleId <= 0)
                throw new InvalidOperationException("El detalle es inválido.");

            var ctx = await repository.ObtenerDetalleContexto(detalleId);
            if (ctx == null)
                throw new InvalidOperationException("Detalle no encontrado.");

            int estadoAceptadoId = await repository.ObtenerEstadoDetalleId("Aceptado manualmente");
            await repository.ActualizarEstadoDetalle(detalleId, estadoAceptadoId);

            await RecalcularEstado(ctx.CON_Conciliacion);
        }

        public async Task RecalcularEstado(int conciliacionId)
        {
            var cabecera = await ObtenerPorId(conciliacionId);
            if (cabecera == null)
                throw new InvalidOperationException("Conciliación no encontrada.");

            bool listaParaCerrar =
                cabecera.PendientesEnLibros == 0 &&
                cabecera.PendientesEnBanco == 0 &&
                cabecera.DiferenciaMonto == 0 &&
                cabecera.DiferenciaFecha == 0 &&
                Math.Abs(cabecera.SaldoBancoAjustado - cabecera.SaldoLibrosAjustado) <= 0.01m;

            int estadoId = listaParaCerrar
                ? await repository.ObtenerEstadoConciliacionId("Conciliada")
                : await repository.ObtenerEstadoConciliacionId("Con diferencias");

            await repository.ActualizarEstadoConciliacion(conciliacionId, estadoId);
        }

        public async Task Cerrar(int conciliacionId)
        {
            if (conciliacionId <= 0)
                throw new InvalidOperationException("La conciliación es inválida.");

            var cabecera = await ObtenerPorId(conciliacionId);
            if (cabecera == null)
                throw new InvalidOperationException("Conciliación no encontrada.");

            bool listaParaCerrar =
                cabecera.PendientesEnLibros == 0 &&
                cabecera.PendientesEnBanco == 0 &&
                cabecera.DiferenciaMonto == 0 &&
                cabecera.DiferenciaFecha == 0 &&
                Math.Abs(cabecera.SaldoBancoAjustado - cabecera.SaldoLibrosAjustado) <= 0.01m;

            if (!listaParaCerrar)
                throw new InvalidOperationException("La conciliación aún tiene diferencias pendientes y no puede cerrarse.");

            int estadoCerradaId = await repository.ObtenerEstadoConciliacionId("Cerrada");
            await repository.ActualizarEstadoConciliacion(conciliacionId, estadoCerradaId);
        }

        private void ValidarEntrada(ProcesarConciliacionDTO dto)
        {
            if (dto.CUB_Cuenta <= 0)
                throw new InvalidOperationException("La cuenta es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.CON_Periodo))
                throw new InvalidOperationException("El período es obligatorio.");

            if (dto.Archivo == null || dto.Archivo.Length == 0)
                throw new InvalidOperationException("Debes cargar un archivo CSV.");

            if (!dto.Archivo.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Por ahora solo se admite CSV.");
        }

        private (DateTime FechaInicio, DateTime FechaFin) ObtenerRangoPeriodo(string periodo)
        {
            if (!DateTime.TryParseExact(
                periodo + "-01",
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var fechaInicio))
            {
                throw new InvalidOperationException("El período debe venir en formato yyyy-MM.");
            }

            var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            return (fechaInicio, fechaFin);
        }

        private async Task<List<MovimientoTemporalImportDTO>> LeerCsv(Microsoft.AspNetCore.Http.IFormFile archivo)
        {
            var result = new List<MovimientoTemporalImportDTO>();

            using var stream = archivo.OpenReadStream();
            using var reader = new StreamReader(stream);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null,
                BadDataFound = null
            };

            using var csv = new CsvReader(reader, config);
            using var dr = new CsvDataReader(csv);

            var dataTable = new System.Data.DataTable();
            dataTable.Load(dr);

            int i = 1;

            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                var fecha = ParseFecha(row, "Fecha");
                var descripcion = ParseString(row, "Descripcion");
                var referencia = ParseNullableString(row, "Referencia");
                var debito = ParseDecimal(row, "Debito");
                var credito = ParseDecimal(row, "Credito");
                var saldo = ParseDecimal(row, "Saldo");

                if (fecha == DateTime.MinValue && string.IsNullOrWhiteSpace(descripcion))
                    continue;

                result.Add(new MovimientoTemporalImportDTO
                {
                    TempKey = i,
                    MTE_Fecha = fecha,
                    MTE_Descripcion = descripcion ?? string.Empty,
                    MTE_Referencia = referencia,
                    MTE_Debito = debito,
                    MTE_Credito = credito,
                    MTE_Saldo = saldo
                });

                i++;
            }

            return result;
        }

        private DateTime ParseFecha(System.Data.DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column))
                throw new InvalidOperationException($"El CSV debe contener la columna '{column}'.");

            var raw = row[column]?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(raw))
                return DateTime.MinValue;

            string[] formats =
            {
                "dd/MM/yyyy",
                "d/M/yyyy",
                "yyyy-MM-dd",
                "MM/dd/yyyy",
                "dd-MM-yyyy"
            };

            if (DateTime.TryParseExact(raw, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var value))
                return value;

            if (DateTime.TryParse(raw, out value))
                return value;

            throw new InvalidOperationException($"No se pudo convertir la fecha '{raw}'.");
        }

        private string? ParseString(System.Data.DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column))
                throw new InvalidOperationException($"El CSV debe contener la columna '{column}'.");

            return row[column]?.ToString()?.Trim();
        }

        private string? ParseNullableString(System.Data.DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column))
                return null;

            return row[column]?.ToString()?.Trim();
        }

        private decimal ParseDecimal(System.Data.DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column))
                throw new InvalidOperationException($"El CSV debe contener la columna '{column}'.");

            var raw = row[column]?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(raw))
                return 0m;

            raw = raw.Replace(",", "");
            raw = raw.Replace("Q", "", StringComparison.OrdinalIgnoreCase);

            if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                return value;

            if (decimal.TryParse(raw, NumberStyles.Any, new CultureInfo("es-GT"), out value))
                return value;

            throw new InvalidOperationException($"No se pudo convertir el número '{raw}' en la columna '{column}'.");
        }

        private decimal ObtenerMontoFirmadoSistema(MovimientoSistemaConciliacionDTO mov)
        {
            bool esIngreso = string.Equals(mov.TIM_Descripcion?.Trim(), "Ingreso", StringComparison.OrdinalIgnoreCase);
            return esIngreso ? mov.MOV_Monto : (mov.MOV_Monto * -1m);
        }

        private decimal ObtenerMontoFirmadoTemporal(MovimientoTemporalImportDTO mov)
        {
            if (mov.MTE_Credito > 0)
                return mov.MTE_Credito;

            if (mov.MTE_Debito > 0)
                return mov.MTE_Debito * -1m;

            return 0m;
        }

        private string Normalizar(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return valor.Trim().Replace(" ", "").ToUpperInvariant();
        }

        private bool EsEnTransito(MovimientoSistemaConciliacionDTO mov, DateTime fechaFin)
        {
            if (string.Equals(mov.ESM_Descripcion?.Trim(), "Pendiente", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(mov.MEM_Descripcion?.Trim(), "Cheque", StringComparison.OrdinalIgnoreCase))
                return true;

            if ((fechaFin.Date - mov.MOV_Fecha.Date).Days <= 3)
                return true;

            return false;
        }

        private void EnriquecerDocumentoConciliacion(
            ConciliacionResponseDTO cabecera,
            List<DetalleConciliacionResponseDTO> detalle)
        {
            decimal depositosTransito = 0m;
            decimal chequesCirculacion = 0m;
            decimal erroresBancarios = 0m;
            decimal ajustesContables = 0m;

            foreach (var item in detalle)
            {
                string estado = (item.EDC_Descripcion ?? string.Empty).Trim().ToUpperInvariant();

                if ((estado == "PENDIENTE EN BANCO" || estado == "EN TRANSITO") && item.MOV_Monto.HasValue)
                {
                    bool esIngreso = string.Equals(item.TIM_Descripcion?.Trim(), "Ingreso", StringComparison.OrdinalIgnoreCase);
                    bool esCheque = string.Equals(item.MEM_Descripcion?.Trim(), "Cheque", StringComparison.OrdinalIgnoreCase);

                    if (esIngreso)
                        depositosTransito += item.MOV_Monto.Value;
                    else if (esCheque)
                        chequesCirculacion += item.MOV_Monto.Value;
                }

                if ((estado == "PENDIENTE EN LIBROS" || estado == "AJUSTADO") &&
                    (item.MTE_Debito.GetValueOrDefault() > 0 || item.MTE_Credito.GetValueOrDefault() > 0))
                {
                    decimal ajuste = item.MTE_Credito.GetValueOrDefault() - item.MTE_Debito.GetValueOrDefault();
                    ajustesContables += ajuste;
                }

                if (estado == "DIFERENCIA DE MONTO" && item.MOV_Monto.HasValue)
                {
                    decimal montoBanco = item.MTE_Credito.GetValueOrDefault() > 0
                        ? item.MTE_Credito.GetValueOrDefault()
                        : item.MTE_Debito.GetValueOrDefault();

                    erroresBancarios += (montoBanco - item.MOV_Monto.Value);
                }
            }

            cabecera.TotalDepositosTransito = depositosTransito;
            cabecera.TotalChequesCirculacion = chequesCirculacion;
            cabecera.TotalErroresBancarios = erroresBancarios;
            cabecera.TotalAjustesContablesPendientes = ajustesContables;

            cabecera.SaldoBancoAjustado =
                cabecera.CON_Saldo_Banco +
                depositosTransito -
                chequesCirculacion +
                erroresBancarios;

            cabecera.SaldoLibrosAjustado =
                cabecera.CON_Saldo_Libros +
                ajustesContables;
        }

        private decimal ObtenerMontoTemporalAbsoluto(DetalleConciliacionContextDTO ctx)
        {
            if ((ctx.MTE_Credito ?? 0m) > 0)
                return ctx.MTE_Credito!.Value;

            if ((ctx.MTE_Debito ?? 0m) > 0)
                return ctx.MTE_Debito!.Value;

            return 0m;
        }

        private string InferirMedioMovimiento(string? descripcion, string tipo)
        {
            string texto = (descripcion ?? string.Empty).Trim().ToUpperInvariant();

            if (texto.Contains("CHEQUE") || texto.Contains("CH-") || texto.Contains("CHQ"))
                return "Cheque";

            if (texto.Contains("TRANSFER"))
                return "Transferencia a otros bancos";

            if (texto.Contains("INTERES"))
                return "Cargo bancario";

            if (texto.Contains("COMISION") || texto.Contains("CARGO") || texto.Contains("RECARGO") || texto.Contains("MANTENIMIENTO"))
                return "Cargo bancario";

            if (texto.Contains("DEPOSITO") || texto.Contains("ABONO") || texto.Contains("EFECTIVO"))
                return "Efectivo";

            return tipo == "Ingreso" ? "Efectivo" : "Cargo bancario";
        }
    }
}