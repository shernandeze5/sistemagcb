using Dapper;
using GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class ConciliacionRepository : IConciliacionRepository
    {
        private readonly string connectionString;

        public ConciliacionRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }

        private OracleConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public async Task<bool> ExisteCuentaActiva(int cuentaId)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT COUNT(*)
                FROM GCB_CUENTA_BANCARIA
                WHERE CUB_Cuenta = :Id
                  AND CUB_Estado = 'A'";

            return await connection.ExecuteScalarAsync<int>(sql, new { Id = cuentaId }) > 0;
        }

        public async Task<bool> ExisteConciliacionPeriodo(int cuentaId, string periodo)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT COUNT(*)
                FROM GCB_CONCILIACION
                WHERE CUB_Cuenta = :Cuenta
                  AND CON_Periodo = :Periodo";

            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                Cuenta = cuentaId,
                Periodo = periodo
            }) > 0;
        }

        public async Task<IEnumerable<MovimientoSistemaConciliacionDTO>> ObtenerMovimientosSistema(
            int cuentaId,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT
                    m.MOV_Movimiento,
                    m.CUB_Cuenta,
                    m.MOV_Fecha,
                    m.MOV_Numero_Referencia,
                    m.MOV_Descripcion,
                    m.MOV_Monto,
                    tm.TIM_Descripcion,
                    mm.MEM_Descripcion,
                    em.ESM_Descripcion
                FROM GCB_MOVIMIENTO m
                INNER JOIN GCB_TIPO_MOVIMIENTO tm
                    ON tm.TIM_Tipo_Movimiento = m.TIM_Tipo_Movimiento
                INNER JOIN GCB_MEDIO_MOVIMIENTO mm
                    ON mm.MEM_Medio_Movimiento = m.MEM_Medio_Movimiento
                INNER JOIN GCB_ESTADO_MOVIMIENTO em
                    ON em.ESM_Estado_Movimiento = m.ESM_Estado_Movimiento
                WHERE m.CUB_Cuenta = :Cuenta
                  AND TRUNC(m.MOV_Fecha) BETWEEN :FechaInicio AND :FechaFin
                  AND UPPER(TRIM(em.ESM_Descripcion)) <> 'ANULADO'
                ORDER BY m.MOV_Fecha, m.MOV_Movimiento";

            return await connection.QueryAsync<MovimientoSistemaConciliacionDTO>(sql, new
            {
                Cuenta = cuentaId,
                FechaInicio = fechaInicio.Date,
                FechaFin = fechaFin.Date
            });
        }

        public async Task<decimal> ObtenerSaldoLibrosAlCorte(int cuentaId, DateTime fechaCorte)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT NVL(x.MOV_Saldo, 0)
                FROM (
                    SELECT MOV_Saldo
                    FROM GCB_MOVIMIENTO
                    WHERE CUB_Cuenta = :Cuenta
                      AND TRUNC(MOV_Fecha) <= :FechaCorte
                    ORDER BY MOV_Fecha DESC, MOV_Movimiento DESC
                ) x
                WHERE ROWNUM = 1";

            return await connection.ExecuteScalarAsync<decimal>(sql, new
            {
                Cuenta = cuentaId,
                FechaCorte = fechaCorte.Date
            });
        }

        public async Task<decimal> ObtenerSaldoActualCuenta(int cuentaId)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT NVL(CUB_Saldo_Actual, 0)
                FROM GCB_CUENTA_BANCARIA
                WHERE CUB_Cuenta = :Cuenta";

            return await connection.ExecuteScalarAsync<decimal>(sql, new { Cuenta = cuentaId });
        }

        public async Task<int> ObtenerEstadoConciliacionId(string descripcion)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT ECO_Estado_Conciliacion
                FROM GCB_ESTADO_CONCILIACION
                WHERE UPPER(TRIM(ECO_Descripcion)) = UPPER(TRIM(:Descripcion))
                  AND ECO_Estado = 'A'
                FETCH FIRST 1 ROWS ONLY";

            var id = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Descripcion = descripcion });

            if (!id.HasValue)
                throw new Exception($"No existe el estado de conciliación '{descripcion}'.");

            return id.Value;
        }

        public async Task<int> ObtenerEstadoDetalleId(string descripcion)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT EDC_Estado_Detalle_Conciliacion
                FROM GCB_ESTADO_DETALLE_CONCILIACION
                WHERE UPPER(TRIM(EDC_Descripcion)) = UPPER(TRIM(:Descripcion))
                  AND EDC_Estado = 'A'
                FETCH FIRST 1 ROWS ONLY";

            var id = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Descripcion = descripcion });

            if (!id.HasValue)
                throw new Exception($"No existe el estado de detalle '{descripcion}'.");

            return id.Value;
        }

        public async Task<int> ObtenerTipoMovimientoId(string descripcion)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT TIM_Tipo_Movimiento
                FROM GCB_TIPO_MOVIMIENTO
                WHERE UPPER(TRIM(TIM_Descripcion)) = UPPER(TRIM(:Descripcion))
                  AND TIM_Estado = 'A'
                FETCH FIRST 1 ROWS ONLY";

            var id = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Descripcion = descripcion });

            if (!id.HasValue)
                throw new Exception($"No existe el tipo de movimiento '{descripcion}'.");

            return id.Value;
        }

        public async Task<int> ObtenerMedioMovimientoId(string descripcion)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT MEM_Medio_Movimiento
                FROM GCB_MEDIO_MOVIMIENTO
                WHERE UPPER(TRIM(MEM_Descripcion)) = UPPER(TRIM(:Descripcion))
                  AND MEM_Estado = 'A'
                FETCH FIRST 1 ROWS ONLY";

            var id = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Descripcion = descripcion });

            if (!id.HasValue)
                throw new Exception($"No existe el medio de movimiento '{descripcion}'.");

            return id.Value;
        }

        public async Task<int> ObtenerEstadoMovimientoId(string descripcion)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT ESM_Estado_Movimiento
                FROM GCB_ESTADO_MOVIMIENTO
                WHERE UPPER(TRIM(ESM_Descripcion)) = UPPER(TRIM(:Descripcion))
                  AND ESM_Estado = 'A'
                FETCH FIRST 1 ROWS ONLY";

            var id = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Descripcion = descripcion });

            if (!id.HasValue)
                throw new Exception($"No existe el estado de movimiento '{descripcion}'.");

            return id.Value;
        }

        public async Task<int> GuardarProcesoConciliacion(GuardarConciliacionDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var trx = connection.BeginTransaction();

            try
            {
                int archivoId;
                int conciliacionId;

                var sqlArchivo = @"
                    INSERT INTO GCB_ARCHIVO_CONCILIACION
                    (
                        CUB_Cuenta,
                        ARC_Nombre_Archivo,
                        ARC_Periodo,
                        ARC_Fecha_Carga,
                        ARC_Estado
                    )
                    VALUES
                    (
                        :CUB_Cuenta,
                        :ARC_Nombre_Archivo,
                        :ARC_Periodo,
                        SYSDATE,
                        'A'
                    )
                    RETURNING ARC_Archivo_Conciliacion INTO :Id";

                var parArchivo = new DynamicParameters();
                parArchivo.Add("CUB_Cuenta", dto.CUB_Cuenta);
                parArchivo.Add("ARC_Nombre_Archivo", dto.ARC_Nombre_Archivo);
                parArchivo.Add("ARC_Periodo", dto.CON_Periodo);
                parArchivo.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(sqlArchivo, parArchivo, trx);
                archivoId = parArchivo.Get<int>("Id");

                var tempMap = new Dictionary<int, int>();

                foreach (var temp in dto.Temporales)
                {
                    var sqlTemp = @"
                        INSERT INTO GCB_MOVIMIENTO_TEMPORAL
                        (
                            ARC_Archivo_Conciliacion,
                            MTE_Fecha,
                            MTE_Descripcion,
                            MTE_Referencia,
                            MTE_Debito,
                            MTE_Credito,
                            MTE_Saldo
                        )
                        VALUES
                        (
                            :ARC_Archivo_Conciliacion,
                            :MTE_Fecha,
                            :MTE_Descripcion,
                            :MTE_Referencia,
                            :MTE_Debito,
                            :MTE_Credito,
                            :MTE_Saldo
                        )
                        RETURNING MTE_Movimiento_Temporal INTO :Id";

                    var parTemp = new DynamicParameters();
                    parTemp.Add("ARC_Archivo_Conciliacion", archivoId);
                    parTemp.Add("MTE_Fecha", temp.MTE_Fecha);
                    parTemp.Add("MTE_Descripcion", temp.MTE_Descripcion);
                    parTemp.Add("MTE_Referencia", temp.MTE_Referencia);
                    parTemp.Add("MTE_Debito", temp.MTE_Debito);
                    parTemp.Add("MTE_Credito", temp.MTE_Credito);
                    parTemp.Add("MTE_Saldo", temp.MTE_Saldo);
                    parTemp.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(sqlTemp, parTemp, trx);
                    tempMap[temp.TempKey] = parTemp.Get<int>("Id");
                }

                var sqlConciliacion = @"
                    INSERT INTO GCB_CONCILIACION
                    (
                        CUB_Cuenta,
                        CON_Periodo,
                        CON_Saldo_Banco,
                        CON_Saldo_Libros,
                        CON_Diferencia,
                        CON_Fecha_Conciliacion,
                        ECO_Estado_Conciliacion
                    )
                    VALUES
                    (
                        :CUB_Cuenta,
                        :CON_Periodo,
                        :CON_Saldo_Banco,
                        :CON_Saldo_Libros,
                        :CON_Diferencia,
                        SYSDATE,
                        :ECO_Estado_Conciliacion
                    )
                    RETURNING CON_Conciliacion INTO :Id";

                var parConc = new DynamicParameters();
                parConc.Add("CUB_Cuenta", dto.CUB_Cuenta);
                parConc.Add("CON_Periodo", dto.CON_Periodo);
                parConc.Add("CON_Saldo_Banco", dto.CON_Saldo_Banco);
                parConc.Add("CON_Saldo_Libros", dto.CON_Saldo_Libros);
                parConc.Add("CON_Diferencia", dto.CON_Diferencia);
                parConc.Add("ECO_Estado_Conciliacion", dto.ECO_Estado_Conciliacion);
                parConc.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(sqlConciliacion, parConc, trx);
                conciliacionId = parConc.Get<int>("Id");

                foreach (var det in dto.Detalles)
                {
                    int? tempId = null;

                    if (det.TempKey.HasValue && tempMap.ContainsKey(det.TempKey.Value))
                        tempId = tempMap[det.TempKey.Value];

                    var sqlDetalle = @"
                        INSERT INTO GCB_DETALLE_CONCILIACION
                        (
                            CON_Conciliacion,
                            MOV_Movimiento,
                            MTE_Movimiento_Temporal,
                            EDC_Estado_Detalle_Conciliacion,
                            DCO_Fecha_Creacion
                        )
                        VALUES
                        (
                            :CON_Conciliacion,
                            :MOV_Movimiento,
                            :MTE_Movimiento_Temporal,
                            :EDC_Estado_Detalle_Conciliacion,
                            SYSDATE
                        )";

                    await connection.ExecuteAsync(sqlDetalle, new
                    {
                        CON_Conciliacion = conciliacionId,
                        MOV_Movimiento = det.MOV_Movimiento,
                        MTE_Movimiento_Temporal = tempId,
                        EDC_Estado_Detalle_Conciliacion = det.EDC_Estado_Detalle_Conciliacion
                    }, trx);
                }

                trx.Commit();
                return conciliacionId;
            }
            catch
            {
                trx.Rollback();
                throw;
            }
        }

        public async Task<ConciliacionResponseDTO?> ObtenerPorId(int conciliacionId)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT
                    c.CON_Conciliacion,
                    c.CUB_Cuenta,
                    c.CON_Periodo,
                    c.CON_Saldo_Banco,
                    c.CON_Saldo_Libros,
                    c.CON_Diferencia,
                    c.CON_Fecha_Conciliacion,
                    c.ECO_Estado_Conciliacion,
                    ec.ECO_Descripcion,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'CONCILIADO' THEN 1 ELSE 0 END) AS Conciliados,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'PENDIENTE EN LIBROS' THEN 1 ELSE 0 END) AS PendientesEnLibros,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'PENDIENTE EN BANCO' THEN 1 ELSE 0 END) AS PendientesEnBanco,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'EN TRANSITO' THEN 1 ELSE 0 END) AS EnTransito,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'DIFERENCIA DE MONTO' THEN 1 ELSE 0 END) AS DiferenciaMonto,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'DIFERENCIA DE FECHA' THEN 1 ELSE 0 END) AS DiferenciaFecha,
                    0 AS TotalDepositosTransito,
                    0 AS TotalChequesCirculacion,
                    0 AS TotalErroresBancarios,
                    0 AS TotalAjustesContablesPendientes,
                    0 AS SaldoBancoAjustado,
                    0 AS SaldoLibrosAjustado
                FROM GCB_CONCILIACION c
                INNER JOIN GCB_ESTADO_CONCILIACION ec
                    ON ec.ECO_Estado_Conciliacion = c.ECO_Estado_Conciliacion
                LEFT JOIN GCB_DETALLE_CONCILIACION dc
                    ON dc.CON_Conciliacion = c.CON_Conciliacion
                LEFT JOIN GCB_ESTADO_DETALLE_CONCILIACION ed
                    ON ed.EDC_Estado_Detalle_Conciliacion = dc.EDC_Estado_Detalle_Conciliacion
                WHERE c.CON_Conciliacion = :Id
                GROUP BY
                    c.CON_Conciliacion,
                    c.CUB_Cuenta,
                    c.CON_Periodo,
                    c.CON_Saldo_Banco,
                    c.CON_Saldo_Libros,
                    c.CON_Diferencia,
                    c.CON_Fecha_Conciliacion,
                    c.ECO_Estado_Conciliacion,
                    ec.ECO_Descripcion";

            return await connection.QueryFirstOrDefaultAsync<ConciliacionResponseDTO>(sql, new { Id = conciliacionId });
        }

        public async Task<IEnumerable<DetalleConciliacionResponseDTO>> ObtenerDetalle(int conciliacionId)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT
                    dc.DCO_Detalle_Conciliacion,
                    dc.CON_Conciliacion,
                    dc.MOV_Movimiento,
                    dc.MTE_Movimiento_Temporal,
                    m.MOV_Fecha,
                    m.MOV_Numero_Referencia,
                    m.MOV_Descripcion,
                    m.MOV_Monto,
                    tm.TIM_Descripcion,
                    mm.MEM_Descripcion,
                    mt.MTE_Fecha,
                    mt.MTE_Referencia,
                    mt.MTE_Descripcion,
                    mt.MTE_Debito,
                    mt.MTE_Credito,
                    mt.MTE_Saldo,
                    dc.EDC_Estado_Detalle_Conciliacion,
                    ed.EDC_Descripcion
                FROM GCB_DETALLE_CONCILIACION dc
                INNER JOIN GCB_ESTADO_DETALLE_CONCILIACION ed
                    ON ed.EDC_Estado_Detalle_Conciliacion = dc.EDC_Estado_Detalle_Conciliacion
                LEFT JOIN GCB_MOVIMIENTO m
                    ON m.MOV_Movimiento = dc.MOV_Movimiento
                LEFT JOIN GCB_TIPO_MOVIMIENTO tm
                    ON tm.TIM_Tipo_Movimiento = m.TIM_Tipo_Movimiento
                LEFT JOIN GCB_MEDIO_MOVIMIENTO mm
                    ON mm.MEM_Medio_Movimiento = m.MEM_Medio_Movimiento
                LEFT JOIN GCB_MOVIMIENTO_TEMPORAL mt
                    ON mt.MTE_Movimiento_Temporal = dc.MTE_Movimiento_Temporal
                WHERE dc.CON_Conciliacion = :Id
                ORDER BY dc.DCO_Detalle_Conciliacion";

            return await connection.QueryAsync<DetalleConciliacionResponseDTO>(sql, new { Id = conciliacionId });
        }

        public async Task<IEnumerable<ConciliacionResponseDTO>> ObtenerPorCuenta(int cuentaId)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT
                    c.CON_Conciliacion,
                    c.CUB_Cuenta,
                    c.CON_Periodo,
                    c.CON_Saldo_Banco,
                    c.CON_Saldo_Libros,
                    c.CON_Diferencia,
                    c.CON_Fecha_Conciliacion,
                    c.ECO_Estado_Conciliacion,
                    ec.ECO_Descripcion,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'CONCILIADO' THEN 1 ELSE 0 END) AS Conciliados,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'PENDIENTE EN LIBROS' THEN 1 ELSE 0 END) AS PendientesEnLibros,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'PENDIENTE EN BANCO' THEN 1 ELSE 0 END) AS PendientesEnBanco,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'EN TRANSITO' THEN 1 ELSE 0 END) AS EnTransito,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'DIFERENCIA DE MONTO' THEN 1 ELSE 0 END) AS DiferenciaMonto,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'DIFERENCIA DE FECHA' THEN 1 ELSE 0 END) AS DiferenciaFecha,
                    0 AS TotalDepositosTransito,
                    0 AS TotalChequesCirculacion,
                    0 AS TotalErroresBancarios,
                    0 AS TotalAjustesContablesPendientes,
                    0 AS SaldoBancoAjustado,
                    0 AS SaldoLibrosAjustado
                FROM GCB_CONCILIACION c
                INNER JOIN GCB_ESTADO_CONCILIACION ec
                    ON ec.ECO_Estado_Conciliacion = c.ECO_Estado_Conciliacion
                LEFT JOIN GCB_DETALLE_CONCILIACION dc
                    ON dc.CON_Conciliacion = c.CON_Conciliacion
                LEFT JOIN GCB_ESTADO_DETALLE_CONCILIACION ed
                    ON ed.EDC_Estado_Detalle_Conciliacion = dc.EDC_Estado_Detalle_Conciliacion
                WHERE c.CUB_Cuenta = :Cuenta
                GROUP BY
                    c.CON_Conciliacion,
                    c.CUB_Cuenta,
                    c.CON_Periodo,
                    c.CON_Saldo_Banco,
                    c.CON_Saldo_Libros,
                    c.CON_Diferencia,
                    c.CON_Fecha_Conciliacion,
                    c.ECO_Estado_Conciliacion,
                    ec.ECO_Descripcion
                ORDER BY c.CON_Fecha_Conciliacion DESC";

            return await connection.QueryAsync<ConciliacionResponseDTO>(sql, new { Cuenta = cuentaId });
        }

        public async Task<IEnumerable<ConciliacionResponseDTO>> ObtenerTodas()
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT
                    c.CON_Conciliacion,
                    c.CUB_Cuenta,
                    c.CON_Periodo,
                    c.CON_Saldo_Banco,
                    c.CON_Saldo_Libros,
                    c.CON_Diferencia,
                    c.CON_Fecha_Conciliacion,
                    c.ECO_Estado_Conciliacion,
                    ec.ECO_Descripcion,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'CONCILIADO' THEN 1 ELSE 0 END) AS Conciliados,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'PENDIENTE EN LIBROS' THEN 1 ELSE 0 END) AS PendientesEnLibros,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'PENDIENTE EN BANCO' THEN 1 ELSE 0 END) AS PendientesEnBanco,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'EN TRANSITO' THEN 1 ELSE 0 END) AS EnTransito,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'DIFERENCIA DE MONTO' THEN 1 ELSE 0 END) AS DiferenciaMonto,
                    SUM(CASE WHEN UPPER(TRIM(ed.EDC_Descripcion)) = 'DIFERENCIA DE FECHA' THEN 1 ELSE 0 END) AS DiferenciaFecha,
                    0 AS TotalDepositosTransito,
                    0 AS TotalChequesCirculacion,
                    0 AS TotalErroresBancarios,
                    0 AS TotalAjustesContablesPendientes,
                    0 AS SaldoBancoAjustado,
                    0 AS SaldoLibrosAjustado
                FROM GCB_CONCILIACION c
                INNER JOIN GCB_ESTADO_CONCILIACION ec
                    ON ec.ECO_Estado_Conciliacion = c.ECO_Estado_Conciliacion
                LEFT JOIN GCB_DETALLE_CONCILIACION dc
                    ON dc.CON_Conciliacion = c.CON_Conciliacion
                LEFT JOIN GCB_ESTADO_DETALLE_CONCILIACION ed
                    ON ed.EDC_Estado_Detalle_Conciliacion = dc.EDC_Estado_Detalle_Conciliacion
                GROUP BY
                    c.CON_Conciliacion,
                    c.CUB_Cuenta,
                    c.CON_Periodo,
                    c.CON_Saldo_Banco,
                    c.CON_Saldo_Libros,
                    c.CON_Diferencia,
                    c.CON_Fecha_Conciliacion,
                    c.ECO_Estado_Conciliacion,
                    ec.ECO_Descripcion
                ORDER BY c.CON_Fecha_Conciliacion DESC";

            return await connection.QueryAsync<ConciliacionResponseDTO>(sql);
        }

        public async Task<DetalleConciliacionContextDTO?> ObtenerDetalleContexto(int detalleId)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT
                    dc.DCO_Detalle_Conciliacion,
                    dc.CON_Conciliacion,
                    c.CUB_Cuenta,
                    dc.MOV_Movimiento,
                    dc.MTE_Movimiento_Temporal,
                    m.MOV_Fecha,
                    m.MOV_Numero_Referencia,
                    m.MOV_Descripcion,
                    m.MOV_Monto,
                    tm.TIM_Descripcion,
                    mm.MEM_Descripcion,
                    mt.MTE_Fecha,
                    mt.MTE_Referencia,
                    mt.MTE_Descripcion,
                    mt.MTE_Debito,
                    mt.MTE_Credito,
                    mt.MTE_Saldo,
                    dc.EDC_Estado_Detalle_Conciliacion,
                    ed.EDC_Descripcion
                FROM GCB_DETALLE_CONCILIACION dc
                INNER JOIN GCB_CONCILIACION c
                    ON c.CON_Conciliacion = dc.CON_Conciliacion
                INNER JOIN GCB_ESTADO_DETALLE_CONCILIACION ed
                    ON ed.EDC_Estado_Detalle_Conciliacion = dc.EDC_Estado_Detalle_Conciliacion
                LEFT JOIN GCB_MOVIMIENTO m
                    ON m.MOV_Movimiento = dc.MOV_Movimiento
                LEFT JOIN GCB_TIPO_MOVIMIENTO tm
                    ON tm.TIM_Tipo_Movimiento = m.TIM_Tipo_Movimiento
                LEFT JOIN GCB_MEDIO_MOVIMIENTO mm
                    ON mm.MEM_Medio_Movimiento = m.MEM_Medio_Movimiento
                LEFT JOIN GCB_MOVIMIENTO_TEMPORAL mt
                    ON mt.MTE_Movimiento_Temporal = dc.MTE_Movimiento_Temporal
                WHERE dc.DCO_Detalle_Conciliacion = :Id";

            return await connection.QueryFirstOrDefaultAsync<DetalleConciliacionContextDTO>(sql, new { Id = detalleId });
        }

        public async Task ActualizarEstadoDetalle(int detalleId, int estadoDetalleId)
        {
            using var connection = GetConnection();

            const string sql = @"
                UPDATE GCB_DETALLE_CONCILIACION
                SET EDC_Estado_Detalle_Conciliacion = :Estado
                WHERE DCO_Detalle_Conciliacion = :Id";

            await connection.ExecuteAsync(sql, new
            {
                Id = detalleId,
                Estado = estadoDetalleId
            });
        }

        public async Task ActualizarEstadoConciliacion(int conciliacionId, int estadoConciliacionId)
        {
            using var connection = GetConnection();

            const string sql = @"
                UPDATE GCB_CONCILIACION
                SET ECO_Estado_Conciliacion = :Estado
                WHERE CON_Conciliacion = :Id";

            await connection.ExecuteAsync(sql, new
            {
                Id = conciliacionId,
                Estado = estadoConciliacionId
            });
        }

        public async Task<bool> ExisteMovimientoCuentaPorReferenciaMonto(int cuentaId, string referencia, decimal monto)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT COUNT(*)
                FROM GCB_MOVIMIENTO
                WHERE CUB_Cuenta = :Cuenta
                  AND UPPER(TRIM(NVL(MOV_Numero_Referencia, ''))) = UPPER(TRIM(:Referencia))
                  AND MOV_Monto = :Monto";

            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                Cuenta = cuentaId,
                Referencia = referencia,
                Monto = monto
            }) > 0;
        }

        public async Task<int> CrearMovimientoDesdeBanco(
            int cuentaId,
            DateTime fecha,
            string? referencia,
            string descripcion,
            decimal monto,
            int tipoMovimientoId,
            int medioMovimientoId,
            int estadoMovimientoId)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var trx = connection.BeginTransaction();

            try
            {
                decimal saldoActual = await connection.ExecuteScalarAsync<decimal>(@"
                    SELECT NVL(CUB_Saldo_Actual, 0)
                    FROM GCB_CUENTA_BANCARIA
                    WHERE CUB_Cuenta = :Cuenta",
                    new { Cuenta = cuentaId }, trx);

                string? tipoDescripcion = await connection.ExecuteScalarAsync<string>(@"
                    SELECT TIM_Descripcion
                    FROM GCB_TIPO_MOVIMIENTO
                    WHERE TIM_Tipo_Movimiento = :Id",
                    new { Id = tipoMovimientoId }, trx);

                bool esIngreso = string.Equals(tipoDescripcion?.Trim(), "Ingreso", StringComparison.OrdinalIgnoreCase);
                decimal saldoNuevo = esIngreso ? saldoActual + monto : saldoActual - monto;

                if (!esIngreso && saldoActual < monto)
                    throw new Exception("Saldo insuficiente para registrar el movimiento desde conciliación.");

                const string sql = @"
                    INSERT INTO GCB_MOVIMIENTO
                    (
                        CUB_Cuenta,
                        PER_Persona,
                        TIM_Tipo_Movimiento,
                        MEM_Medio_Movimiento,
                        RCA_Regla_Recargo,
                        MOV_Fecha,
                        MOV_Numero_Referencia,
                        MOV_Descripcion,
                        MOV_Monto,
                        MOV_Monto_Origen,
                        MOV_Saldo,
                        ESM_Estado_Movimiento,
                        MOV_Fecha_Creacion
                    )
                    VALUES
                    (
                        :CUB_Cuenta,
                        NULL,
                        :TIM_Tipo_Movimiento,
                        :MEM_Medio_Movimiento,
                        NULL,
                        :MOV_Fecha,
                        :MOV_Numero_Referencia,
                        :MOV_Descripcion,
                        :MOV_Monto,
                        :MOV_Monto_Origen,
                        :MOV_Saldo,
                        :ESM_Estado_Movimiento,
                        SYSDATE
                    )
                    RETURNING MOV_Movimiento INTO :Id";

                var p = new DynamicParameters();
                p.Add("CUB_Cuenta", cuentaId);
                p.Add("TIM_Tipo_Movimiento", tipoMovimientoId);
                p.Add("MEM_Medio_Movimiento", medioMovimientoId);
                p.Add("MOV_Fecha", fecha);
                p.Add("MOV_Numero_Referencia", referencia);
                p.Add("MOV_Descripcion", descripcion);
                p.Add("MOV_Monto", monto);
                p.Add("MOV_Monto_Origen", monto);
                p.Add("MOV_Saldo", saldoNuevo);
                p.Add("ESM_Estado_Movimiento", estadoMovimientoId);
                p.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(sql, p, trx);

                await connection.ExecuteAsync(@"
                    UPDATE GCB_CUENTA_BANCARIA
                    SET CUB_Saldo_Actual = :Saldo
                    WHERE CUB_Cuenta = :Cuenta",
                    new
                    {
                        Cuenta = cuentaId,
                        Saldo = saldoNuevo
                    }, trx);

                trx.Commit();
                return p.Get<int>("Id");
            }
            catch
            {
                trx.Rollback();
                throw;
            }
        }
    }
}