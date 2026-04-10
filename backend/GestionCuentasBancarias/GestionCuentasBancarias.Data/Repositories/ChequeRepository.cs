using Dapper;
using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using System.Data;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class ChequeRepository : IChequeRepository
    {
        private readonly OracleConnectionFactory connectionFactory;

        public ChequeRepository(OracleConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<ChequeResponseDTO>> ObtenerChequesAsync()
        {
            var sql = @"
                SELECT
                    C.CHE_Cheque                AS CHE_Cheque,
                    C.MOV_Movimiento            AS MOV_Movimiento,
                    M.CUB_Cuenta                AS CUB_Cuenta,
                    C.CHQ_Chequera              AS CHQ_Chequera,
                    M.PER_Persona               AS PER_Persona,
                    CASE
                        WHEN P.PER_Razon_Social IS NOT NULL AND TRIM(P.PER_Razon_Social) <> '' THEN P.PER_Razon_Social
                        ELSE TRIM(
                            NVL(P.PER_Primer_Nombre, '') || ' ' ||
                            NVL(P.PER_Segundo_Nombre, '') || ' ' ||
                            NVL(P.PER_Primer_Apellido, '') || ' ' ||
                            NVL(P.PER_Segundo_Apellido, '')
                        )
                    END                         AS Beneficiario,
                    C.CHE_Numero_Cheque         AS CHE_Numero_Cheque,
                    C.CHE_Monto_Letras          AS CHE_Monto_Letras,
                    C.CHE_Fecha_Emision         AS CHE_Fecha_Emision,
                    C.CHE_Fecha_Cobro           AS CHE_Fecha_Cobro,
                    C.CHE_Fecha_Vencimiento     AS CHE_Fecha_Vencimiento,
                    C.CHE_Concepto              AS CHE_Concepto,
                    C.ESC_Estado_Cheque         AS ESC_Estado_Cheque,
                    EC.ESC_Descripcion          AS EstadoCheque,
                    M.MOV_Fecha                 AS MOV_Fecha,
                    NVL(M.MOV_Numero_Referencia, '') AS MOV_Numero_Referencia,
                    NVL(M.MOV_Descripcion, '')       AS MOV_Descripcion,
                    M.MOV_Monto                 AS MOV_Monto,
                    M.MOV_Monto_Origen          AS MOV_Monto_Origen,
                    M.MOV_Saldo                 AS MOV_Saldo,
                    M.TIM_Tipo_Movimiento       AS TIM_Tipo_Movimiento,
                    M.MEM_Medio_Movimiento      AS MEM_Medio_Movimiento,
                    M.ESM_Estado_Movimiento     AS ESM_Estado_Movimiento
                FROM GCB_CHEQUE C
                INNER JOIN GCB_MOVIMIENTO M
                    ON M.MOV_Movimiento = C.MOV_Movimiento
                LEFT JOIN GCB_PERSONA P
                    ON P.PER_Persona = M.PER_Persona
                INNER JOIN GCB_ESTADO_CHEQUE EC
                    ON EC.ESC_Estado_Cheque = C.ESC_Estado_Cheque
                ORDER BY C.CHE_Cheque DESC";

            using var connection = connectionFactory.CreateConnection();
            return await connection.QueryAsync<ChequeResponseDTO>(sql);
        }

        public async Task<IEnumerable<ChequeResponseDTO>> ObtenerChequesPorCuentaAsync(int cuentaId)
        {
            var sql = @"
                SELECT
                    C.CHE_Cheque                AS CHE_Cheque,
                    C.MOV_Movimiento            AS MOV_Movimiento,
                    M.CUB_Cuenta                AS CUB_Cuenta,
                    C.CHQ_Chequera              AS CHQ_Chequera,
                    M.PER_Persona               AS PER_Persona,
                    CASE
                        WHEN P.PER_Razon_Social IS NOT NULL AND TRIM(P.PER_Razon_Social) <> '' THEN P.PER_Razon_Social
                        ELSE TRIM(
                            NVL(P.PER_Primer_Nombre, '') || ' ' ||
                            NVL(P.PER_Segundo_Nombre, '') || ' ' ||
                            NVL(P.PER_Primer_Apellido, '') || ' ' ||
                            NVL(P.PER_Segundo_Apellido, '')
                        )
                    END                         AS Beneficiario,
                    C.CHE_Numero_Cheque         AS CHE_Numero_Cheque,
                    C.CHE_Monto_Letras          AS CHE_Monto_Letras,
                    C.CHE_Fecha_Emision         AS CHE_Fecha_Emision,
                    C.CHE_Fecha_Cobro           AS CHE_Fecha_Cobro,
                    C.CHE_Fecha_Vencimiento     AS CHE_Fecha_Vencimiento,
                    C.CHE_Concepto              AS CHE_Concepto,
                    C.ESC_Estado_Cheque         AS ESC_Estado_Cheque,
                    EC.ESC_Descripcion          AS EstadoCheque,
                    M.MOV_Fecha                 AS MOV_Fecha,
                    NVL(M.MOV_Numero_Referencia, '') AS MOV_Numero_Referencia,
                    NVL(M.MOV_Descripcion, '')       AS MOV_Descripcion,
                    M.MOV_Monto                 AS MOV_Monto,
                    M.MOV_Monto_Origen          AS MOV_Monto_Origen,
                    M.MOV_Saldo                 AS MOV_Saldo,
                    M.TIM_Tipo_Movimiento       AS TIM_Tipo_Movimiento,
                    M.MEM_Medio_Movimiento      AS MEM_Medio_Movimiento,
                    M.ESM_Estado_Movimiento     AS ESM_Estado_Movimiento
                FROM GCB_CHEQUE C
                INNER JOIN GCB_MOVIMIENTO M
                    ON M.MOV_Movimiento = C.MOV_Movimiento
                LEFT JOIN GCB_PERSONA P
                    ON P.PER_Persona = M.PER_Persona
                INNER JOIN GCB_ESTADO_CHEQUE EC
                    ON EC.ESC_Estado_Cheque = C.ESC_Estado_Cheque
                WHERE M.CUB_Cuenta = :CuentaId
                ORDER BY C.CHE_Cheque DESC";

            using var connection = connectionFactory.CreateConnection();
            return await connection.QueryAsync<ChequeResponseDTO>(sql, new { CuentaId = cuentaId });
        }

        public async Task<ChequeResponseDTO?> ObtenerChequePorIdAsync(int id)
        {
            var sql = @"
                SELECT
                    C.CHE_Cheque                AS CHE_Cheque,
                    C.MOV_Movimiento            AS MOV_Movimiento,
                    M.CUB_Cuenta                AS CUB_Cuenta,
                    C.CHQ_Chequera              AS CHQ_Chequera,
                    M.PER_Persona               AS PER_Persona,
                    CASE
                        WHEN P.PER_Razon_Social IS NOT NULL AND TRIM(P.PER_Razon_Social) <> '' THEN P.PER_Razon_Social
                        ELSE TRIM(
                            NVL(P.PER_Primer_Nombre, '') || ' ' ||
                            NVL(P.PER_Segundo_Nombre, '') || ' ' ||
                            NVL(P.PER_Primer_Apellido, '') || ' ' ||
                            NVL(P.PER_Segundo_Apellido, '')
                        )
                    END                         AS Beneficiario,
                    C.CHE_Numero_Cheque         AS CHE_Numero_Cheque,
                    C.CHE_Monto_Letras          AS CHE_Monto_Letras,
                    C.CHE_Fecha_Emision         AS CHE_Fecha_Emision,
                    C.CHE_Fecha_Cobro           AS CHE_Fecha_Cobro,
                    C.CHE_Fecha_Vencimiento     AS CHE_Fecha_Vencimiento,
                    C.CHE_Concepto              AS CHE_Concepto,
                    C.ESC_Estado_Cheque         AS ESC_Estado_Cheque,
                    EC.ESC_Descripcion          AS EstadoCheque,
                    M.MOV_Fecha                 AS MOV_Fecha,
                    NVL(M.MOV_Numero_Referencia, '') AS MOV_Numero_Referencia,
                    NVL(M.MOV_Descripcion, '')       AS MOV_Descripcion,
                    M.MOV_Monto                 AS MOV_Monto,
                    M.MOV_Monto_Origen          AS MOV_Monto_Origen,
                    M.MOV_Saldo                 AS MOV_Saldo,
                    M.TIM_Tipo_Movimiento       AS TIM_Tipo_Movimiento,
                    M.MEM_Medio_Movimiento      AS MEM_Medio_Movimiento,
                    M.ESM_Estado_Movimiento     AS ESM_Estado_Movimiento
                FROM GCB_CHEQUE C
                INNER JOIN GCB_MOVIMIENTO M
                    ON M.MOV_Movimiento = C.MOV_Movimiento
                LEFT JOIN GCB_PERSONA P
                    ON P.PER_Persona = M.PER_Persona
                INNER JOIN GCB_ESTADO_CHEQUE EC
                    ON EC.ESC_Estado_Cheque = C.ESC_Estado_Cheque
                WHERE C.CHE_Cheque = :Id";

            using var connection = connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ChequeResponseDTO>(sql, new { Id = id });
        }

        public async Task<bool> CrearChequeAsync(CreateChequeDTO dto)
        {
            using var connection = connectionFactory.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                await ValidarNumeroChequeDuplicadoAsync(
                    connection,
                    dto.CHQ_Chequera,
                    dto.CHE_Numero_Cheque,
                    transaction
                );

                await ValidarEstadoChequeAsync(connection, dto.ESC_Estado_Cheque, transaction);

                int estadoMovimientoActivoId = await ObtenerEstadoMovimientoIdAsync(connection, "Activo", transaction);
                int tipoMovimientoEgresoId = await ObtenerTipoMovimientoIdAsync(connection, "Egreso", transaction);
                int medioMovimientoChequeId = await ObtenerMedioMovimientoIdAsync(connection, "Cheque", transaction);

                var personaExiste = await connection.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(1)
                    FROM GCB_PERSONA
                    WHERE PER_Persona = :PER_Persona",
                    new { dto.PER_Persona }, transaction);

                if (personaExiste == 0)
                    throw new InvalidOperationException("La persona beneficiaria no existe.");

                var chequeraInfo = await connection.QueryFirstOrDefaultAsync<dynamic>(@"
                    SELECT 
                        CHQ_Chequera,
                        CUB_Cuenta,
                        CHQ_Numero_Desde,
                        CHQ_Numero_Hasta,
                        CHQ_Ultimo_Usado,
                        CHQ_Estado
                    FROM GCB_CHEQUERA
                    WHERE CHQ_Chequera = :CHQ_Chequera",
                    new { dto.CHQ_Chequera }, transaction);

                if (chequeraInfo == null)
                    throw new InvalidOperationException("La chequera no existe.");

                if (Convert.ToInt32(chequeraInfo.CUB_CUENTA) != dto.CUB_Cuenta)
                    throw new InvalidOperationException("La chequera no pertenece a la cuenta enviada.");

                string estadoChequera = Convert.ToString(chequeraInfo.CHQ_ESTADO) ?? string.Empty;

                if (!estadoChequera.Equals("A", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("La chequera está inactiva.");

                int numeroDesde = Convert.ToInt32(chequeraInfo.CHQ_NUMERO_DESDE);
                int numeroHasta = Convert.ToInt32(chequeraInfo.CHQ_NUMERO_HASTA);
                int ultimoUsado = Convert.ToInt32(chequeraInfo.CHQ_ULTIMO_USADO);

                if (ultimoUsado >= numeroHasta)
                    throw new InvalidOperationException("La chequera ya no tiene cheques disponibles.");

                if (!int.TryParse(dto.CHE_Numero_Cheque, out int numeroCheque))
                    throw new InvalidOperationException("El número de cheque debe ser numérico.");

                if (numeroCheque < numeroDesde || numeroCheque > numeroHasta)
                    throw new InvalidOperationException("El número de cheque está fuera del rango de la chequera.");

                int siguienteEsperado = ultimoUsado == 0 ? numeroDesde : ultimoUsado + 1;

                if (numeroCheque != siguienteEsperado)
                    throw new InvalidOperationException($"El siguiente número de cheque válido es {siguienteEsperado}.");

                var saldoActual = await connection.ExecuteScalarAsync<decimal?>(@"
                    SELECT CUB_Saldo_Actual
                    FROM GCB_CUENTA_BANCARIA
                    WHERE CUB_Cuenta = :CUB_Cuenta
                      AND CUB_Estado = 'A'",
                    new { dto.CUB_Cuenta }, transaction);

                if (saldoActual == null)
                    throw new InvalidOperationException("La cuenta bancaria no existe o está inactiva.");

                if (saldoActual.Value < dto.MOV_Monto)
                    throw new InvalidOperationException("La cuenta no tiene saldo suficiente para emitir el cheque.");

                decimal nuevoSaldo = saldoActual.Value - dto.MOV_Monto;

                var movimientoParams = new DynamicParameters();
                movimientoParams.Add("CUB_Cuenta", dto.CUB_Cuenta);
                movimientoParams.Add("PER_Persona", dto.PER_Persona);
                movimientoParams.Add("TIM_Tipo_Movimiento", tipoMovimientoEgresoId);
                movimientoParams.Add("MEM_Medio_Movimiento", medioMovimientoChequeId);
                movimientoParams.Add("MOV_Fecha", dto.CHE_Fecha_Emision);
                movimientoParams.Add("MOV_Numero_Referencia", dto.MOV_Numero_Referencia);
                movimientoParams.Add("MOV_Descripcion", dto.MOV_Descripcion);
                movimientoParams.Add("MOV_Monto", dto.MOV_Monto);
                movimientoParams.Add("MOV_Monto_Origen", dto.MOV_Monto);
                movimientoParams.Add("MOV_Saldo", nuevoSaldo);
                movimientoParams.Add("ESM_Estado_Movimiento", estadoMovimientoActivoId);
                movimientoParams.Add("MOV_Movimiento", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var sqlMovimiento = @"
                    INSERT INTO GCB_MOVIMIENTO
                    (
                        CUB_Cuenta,
                        PER_Persona,
                        TIM_Tipo_Movimiento,
                        MEM_Medio_Movimiento,
                        MOV_Fecha,
                        MOV_Numero_Referencia,
                        MOV_Descripcion,
                        MOV_Monto,
                        MOV_Monto_Origen,
                        ESM_Estado_Movimiento,
                        MOV_Fecha_Creacion,
                        MOV_Saldo
                    )
                    VALUES
                    (
                        :CUB_Cuenta,
                        :PER_Persona,
                        :TIM_Tipo_Movimiento,
                        :MEM_Medio_Movimiento,
                        :MOV_Fecha,
                        :MOV_Numero_Referencia,
                        :MOV_Descripcion,
                        :MOV_Monto,
                        :MOV_Monto_Origen,
                        :ESM_Estado_Movimiento,
                        SYSDATE,
                        :MOV_Saldo
                    )
                    RETURNING MOV_Movimiento INTO :MOV_Movimiento";

                await connection.ExecuteAsync(sqlMovimiento, movimientoParams, transaction);

                int movimientoId = movimientoParams.Get<int>("MOV_Movimiento");

                var chequeParams = new DynamicParameters();
                chequeParams.Add("MOV_Movimiento", movimientoId);
                chequeParams.Add("CHE_Numero_Cheque", dto.CHE_Numero_Cheque);
                chequeParams.Add("CHE_Monto_Letras", dto.CHE_Monto_Letras);
                chequeParams.Add("CHE_Fecha_Emision", dto.CHE_Fecha_Emision);
                chequeParams.Add("CHE_Fecha_Cobro", null);
                chequeParams.Add("CHE_Fecha_Vencimiento", dto.CHE_Fecha_Vencimiento);
                chequeParams.Add("ESC_Estado_Cheque", dto.ESC_Estado_Cheque);
                chequeParams.Add("CHQ_Chequera", dto.CHQ_Chequera);
                chequeParams.Add("CHE_Concepto", dto.CHE_Concepto);

                var sqlCheque = @"
                    INSERT INTO GCB_CHEQUE
                    (
                        MOV_Movimiento,
                        CHE_Numero_Cheque,
                        CHE_Monto_Letras,
                        CHE_Fecha_Emision,
                        CHE_Fecha_Cobro,
                        CHE_Fecha_Vencimiento,
                        ESC_Estado_Cheque,
                        CHE_Fecha_Creacion,
                        CHQ_Chequera,
                        CHE_Concepto
                    )
                    VALUES
                    (
                        :MOV_Movimiento,
                        :CHE_Numero_Cheque,
                        :CHE_Monto_Letras,
                        :CHE_Fecha_Emision,
                        :CHE_Fecha_Cobro,
                        :CHE_Fecha_Vencimiento,
                        :ESC_Estado_Cheque,
                        SYSDATE,
                        :CHQ_Chequera,
                        :CHE_Concepto
                    )";

                var filasCheque = await connection.ExecuteAsync(sqlCheque, chequeParams, transaction);

                await connection.ExecuteAsync(@"
                    UPDATE GCB_CUENTA_BANCARIA
                    SET CUB_Saldo_Actual = :NuevoSaldo
                    WHERE CUB_Cuenta = :CUB_Cuenta",
                    new
                    {
                        NuevoSaldo = nuevoSaldo,
                        CUB_Cuenta = dto.CUB_Cuenta
                    }, transaction);

                await connection.ExecuteAsync(@"
                    UPDATE GCB_CHEQUERA
                    SET CHQ_Ultimo_Usado = :CHQ_Ultimo_Usado
                    WHERE CHQ_Chequera = :CHQ_Chequera",
                    new
                    {
                        CHQ_Ultimo_Usado = numeroCheque,
                        CHQ_Chequera = dto.CHQ_Chequera
                    }, transaction);

                transaction.Commit();
                return filasCheque > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> CambiarEstadoChequeAsync(int chequeId, UpdateDTOCheque dto)
        {
            using var connection = connectionFactory.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                await ValidarEstadoChequeAsync(connection, dto.ESC_Estado_Cheque, transaction);

                var chequeInfo = await connection.QueryFirstOrDefaultAsync<dynamic>(@"
                    SELECT
                        C.CHE_Cheque,
                        C.ESC_Estado_Cheque,
                        C.CHE_Fecha_Cobro,
                        C.MOV_Movimiento,
                        M.CUB_Cuenta,
                        M.MOV_Monto
                    FROM GCB_CHEQUE C
                    INNER JOIN GCB_MOVIMIENTO M
                        ON M.MOV_Movimiento = C.MOV_Movimiento
                    WHERE C.CHE_Cheque = :CHE_Cheque",
                    new { CHE_Cheque = chequeId }, transaction);

                if (chequeInfo == null)
                    throw new InvalidOperationException("El cheque no existe.");

                int estadoActualId = Convert.ToInt32(chequeInfo.ESC_ESTADO_CHEQUE);
                int movimientoId = Convert.ToInt32(chequeInfo.MOV_MOVIMIENTO);
                int cuentaId = Convert.ToInt32(chequeInfo.CUB_CUENTA);
                decimal monto = Convert.ToDecimal(chequeInfo.MOV_MONTO);

                int estadoCanceladoId = await ObtenerEstadoChequeIdPorDescripcionAsync(connection, "Cancelado", transaction);
                int estadoDepositadoId = await ObtenerEstadoChequeIdPorDescripcionAsync(connection, "Cobrado", transaction);

                if (estadoActualId == estadoCanceladoId)
                    throw new InvalidOperationException("No se puede cambiar el estado de un cheque cancelado.");

                if (estadoActualId == estadoDepositadoId && dto.ESC_Estado_Cheque != estadoDepositadoId)
                    throw new InvalidOperationException("No se puede cambiar el estado de un cheque depositado.");

                if (estadoActualId == dto.ESC_Estado_Cheque)
                    throw new InvalidOperationException("El cheque ya tiene ese estado.");

                if (dto.ESC_Estado_Cheque == estadoDepositadoId)
                {
                    var filasDepositado = await connection.ExecuteAsync(@"
                        UPDATE GCB_CHEQUE
                        SET ESC_Estado_Cheque = :ESC_Estado_Cheque,
                            CHE_Fecha_Cobro = SYSDATE
                        WHERE CHE_Cheque = :CHE_Cheque",
                        new
                        {
                            ESC_Estado_Cheque = dto.ESC_Estado_Cheque,
                            CHE_Cheque = chequeId
                        }, transaction);

                    transaction.Commit();
                    return filasDepositado > 0;
                }

                if (dto.ESC_Estado_Cheque == estadoCanceladoId)
                {
                    int estadoMovimientoAnuladoId = await ObtenerEstadoMovimientoIdAsync(connection, "Anulado", transaction);

                    var saldoActual = await connection.ExecuteScalarAsync<decimal?>(@"
                        SELECT CUB_Saldo_Actual
                        FROM GCB_CUENTA_BANCARIA
                        WHERE CUB_Cuenta = :CUB_Cuenta",
                        new { CUB_Cuenta = cuentaId }, transaction);

                    if (saldoActual == null)
                        throw new InvalidOperationException("La cuenta bancaria asociada al cheque no existe.");

                    decimal nuevoSaldo = saldoActual.Value + monto;

                    await connection.ExecuteAsync(@"
                        UPDATE GCB_CHEQUE
                        SET ESC_Estado_Cheque = :ESC_Estado_Cheque
                        WHERE CHE_Cheque = :CHE_Cheque",
                        new
                        {
                            ESC_Estado_Cheque = dto.ESC_Estado_Cheque,
                            CHE_Cheque = chequeId
                        }, transaction);

                    await connection.ExecuteAsync(@"
                        UPDATE GCB_MOVIMIENTO
                        SET ESM_Estado_Movimiento = :ESM_Estado_Movimiento
                        WHERE MOV_Movimiento = :MOV_Movimiento",
                        new
                        {
                            ESM_Estado_Movimiento = estadoMovimientoAnuladoId,
                            MOV_Movimiento = movimientoId
                        }, transaction);

                    await connection.ExecuteAsync(@"
                        UPDATE GCB_CUENTA_BANCARIA
                        SET CUB_Saldo_Actual = :NuevoSaldo
                        WHERE CUB_Cuenta = :CUB_Cuenta",
                        new
                        {
                            NuevoSaldo = nuevoSaldo,
                            CUB_Cuenta = cuentaId
                        }, transaction);

                    transaction.Commit();
                    return true;
                }

                var filas = await connection.ExecuteAsync(@"
                    UPDATE GCB_CHEQUE
                    SET ESC_Estado_Cheque = :ESC_Estado_Cheque
                    WHERE CHE_Cheque = :CHE_Cheque",
                    new
                    {
                        ESC_Estado_Cheque = dto.ESC_Estado_Cheque,
                        CHE_Cheque = chequeId
                    }, transaction);

                transaction.Commit();
                return filas > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private async Task ValidarNumeroChequeDuplicadoAsync(
            IDbConnection connection,
            int chqChequera,
            string numeroCheque,
            IDbTransaction transaction)
        {
            var existe = await connection.ExecuteScalarAsync<int>(@"
                SELECT COUNT(1)
                FROM GCB_CHEQUE
                WHERE CHQ_Chequera = :CHQ_Chequera
                  AND UPPER(TRIM(CHE_Numero_Cheque)) = UPPER(TRIM(:NumeroCheque))",
                new
                {
                    CHQ_Chequera = chqChequera,
                    NumeroCheque = numeroCheque
                },
                transaction);

            if (existe > 0)
                throw new InvalidOperationException("Ya existe un cheque con ese número en la chequera seleccionada.");
        }

        private async Task ValidarEstadoChequeAsync(IDbConnection connection, int estadoId, IDbTransaction transaction)
        {
            var existe = await connection.ExecuteScalarAsync<int>(@"
                SELECT COUNT(1)
                FROM GCB_ESTADO_CHEQUE
                WHERE ESC_Estado_Cheque = :ESC_Estado_Cheque
                  AND ESC_Estado = 'A'",
                new { ESC_Estado_Cheque = estadoId }, transaction);

            if (existe == 0)
                throw new InvalidOperationException("El estado de cheque enviado no existe o está inactivo.");
        }

        private async Task<int> ObtenerEstadoChequeIdPorDescripcionAsync(IDbConnection connection, string descripcion, IDbTransaction transaction)
        {
            var id = await connection.ExecuteScalarAsync<int?>(@"
                SELECT ESC_Estado_Cheque
                FROM GCB_ESTADO_CHEQUE
                WHERE UPPER(TRIM(ESC_Descripcion)) = UPPER(TRIM(:Descripcion))
                  AND ESC_Estado = 'A'",
                new { Descripcion = descripcion }, transaction);

            if (!id.HasValue)
                throw new InvalidOperationException($"No existe el estado de cheque '{descripcion}'.");

            return id.Value;
        }

        private async Task<int> ObtenerEstadoMovimientoIdAsync(IDbConnection connection, string descripcion, IDbTransaction transaction)
        {
            var id = await connection.ExecuteScalarAsync<int?>(@"
                SELECT ESM_Estado_Movimiento
                FROM GCB_ESTADO_MOVIMIENTO
                WHERE UPPER(TRIM(ESM_Descripcion)) = UPPER(TRIM(:Descripcion))
                  AND ESM_Estado = 'A'",
                new { Descripcion = descripcion }, transaction);

            if (!id.HasValue)
                throw new InvalidOperationException($"No existe el estado de movimiento '{descripcion}'.");

            return id.Value;
        }

        private async Task<int> ObtenerTipoMovimientoIdAsync(IDbConnection connection, string descripcion, IDbTransaction transaction)
        {
            var id = await connection.ExecuteScalarAsync<int?>(@"
                SELECT TIM_Tipo_Movimiento
                FROM GCB_TIPO_MOVIMIENTO
                WHERE UPPER(TRIM(TIM_Descripcion)) = UPPER(TRIM(:Descripcion))
                  AND TIM_Estado = 'A'",
                new { Descripcion = descripcion }, transaction);

            if (!id.HasValue)
                throw new InvalidOperationException($"No existe el tipo de movimiento '{descripcion}'.");

            return id.Value;
        }

        private async Task<int> ObtenerMedioMovimientoIdAsync(IDbConnection connection, string descripcion, IDbTransaction transaction)
        {
            var id = await connection.ExecuteScalarAsync<int?>(@"
                SELECT MEM_Medio_Movimiento
                FROM GCB_MEDIO_MOVIMIENTO
                WHERE UPPER(TRIM(MEM_Descripcion)) = UPPER(TRIM(:Descripcion))
                  AND MEM_Estado = 'A'",
                new { Descripcion = descripcion }, transaction);

            if (!id.HasValue)
                throw new InvalidOperationException($"No existe el medio de movimiento '{descripcion}'.");

            return id.Value;
        }
    }
}