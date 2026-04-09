using Dapper;
using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly string connectionString;

        public MovimientoRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }

        private OracleConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public async Task<int> CrearConRecargo(CreateMovimientoDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var trx = connection.BeginTransaction();

            try
            {
                decimal saldoActual = await connection.ExecuteScalarAsync<decimal>(@"
                    SELECT NVL(CUB_Saldo_Actual, 0)
                    FROM GCB_CUENTA_BANCARIA
                    WHERE CUB_Cuenta = :Id",
                    new { Id = dto.CUB_Cuenta }, trx);

                string? tipoDescripcion = await connection.ExecuteScalarAsync<string>(@"
                    SELECT TIM_Descripcion
                    FROM GCB_TIPO_MOVIMIENTO
                    WHERE TIM_Tipo_Movimiento = :Id",
                    new { Id = dto.TIM_Tipo_Movimiento }, trx);

                string? medioDescripcion = await connection.ExecuteScalarAsync<string>(@"
                    SELECT MEM_Descripcion
                    FROM GCB_MEDIO_MOVIMIENTO
                    WHERE MEM_Medio_Movimiento = :Id",
                    new { Id = dto.MEM_Medio_Movimiento }, trx);

                bool esIngreso = string.Equals(tipoDescripcion?.Trim(), "Ingreso", StringComparison.OrdinalIgnoreCase);
                bool esEgreso = string.Equals(tipoDescripcion?.Trim(), "Egreso", StringComparison.OrdinalIgnoreCase);

                if (!esIngreso && !esEgreso)
                    throw new Exception("El tipo de movimiento debe ser Ingreso o Egreso.");

                string referenciaPrincipal = GenerarReferencia(dto.MOV_Numero_Referencia);
                string referenciaRecargo = $"{referenciaPrincipal}-RCG";

                bool aplicaRecargo =
                    esEgreso &&
                    string.Equals(medioDescripcion?.Trim(), "Transferencia a otros bancos", StringComparison.OrdinalIgnoreCase);

                decimal recargo = aplicaRecargo ? 3m : 0m;

                decimal saldoDespuesPrincipal;
                decimal saldoFinal;

                if (esIngreso)
                {
                    saldoDespuesPrincipal = saldoActual + dto.MOV_Monto_Origen;
                    saldoFinal = saldoDespuesPrincipal;
                }
                else
                {
                    decimal totalSalida = dto.MOV_Monto_Origen + recargo;

                    if (saldoActual < totalSalida)
                        throw new Exception("Saldo insuficiente para realizar el movimiento y cubrir el recargo.");

                    saldoDespuesPrincipal = saldoActual - dto.MOV_Monto_Origen;
                    saldoFinal = saldoDespuesPrincipal - recargo;
                }

                int idPrincipal = await InsertarMovimiento(
                    connection,
                    trx,
                    dto.CUB_Cuenta,
                    dto.PER_Persona,
                    dto.TIM_Tipo_Movimiento,
                    dto.MEM_Medio_Movimiento,
                    dto.ESM_Estado_Movimiento,
                    dto.MOV_Fecha,
                    referenciaPrincipal,
                    dto.MOV_Descripcion,
                    dto.MOV_Monto_Origen,
                    dto.MOV_Monto_Origen,
                    saldoDespuesPrincipal);

                if (aplicaRecargo)
                {
                    int medioCargoBancarioId = await ObtenerMedioCargoBancarioId(connection, trx);

                    await InsertarMovimiento(
                        connection,
                        trx,
                        dto.CUB_Cuenta,
                        dto.PER_Persona,
                        dto.TIM_Tipo_Movimiento,
                        medioCargoBancarioId,
                        dto.ESM_Estado_Movimiento,
                        dto.MOV_Fecha,
                        referenciaRecargo,
                        "Recargo por transferencia a otros bancos",
                        recargo,
                        recargo,
                        saldoFinal);
                }

                await connection.ExecuteAsync(@"
                    UPDATE GCB_CUENTA_BANCARIA
                    SET CUB_Saldo_Actual = :Saldo
                    WHERE CUB_Cuenta = :Id",
                    new
                    {
                        Id = dto.CUB_Cuenta,
                        Saldo = saldoFinal
                    }, trx);

                trx.Commit();
                return idPrincipal;
            }
            catch
            {
                trx.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<ResponseMovimientoDTO>> ObtenerTodos()
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT
                    m.MOV_Movimiento,
                    m.CUB_Cuenta,
                    cb.CUB_Numero_Cuenta,
                    m.PER_Persona,
                    CASE
                        WHEN p.PER_Persona IS NULL THEN NULL
                        WHEN p.PER_Razon_Social IS NOT NULL THEN p.PER_Razon_Social
                        ELSE TRIM(
                            NVL(p.PER_Primer_Nombre, '') || ' ' ||
                            NVL(p.PER_Segundo_Nombre, '') || ' ' ||
                            NVL(p.PER_Primer_Apellido, '') || ' ' ||
                            NVL(p.PER_Segundo_Apellido, '')
                        )
                    END AS PER_Nombre_Completo,
                    m.TIM_Tipo_Movimiento,
                    tm.TIM_Descripcion,
                    m.MEM_Medio_Movimiento,
                    mm.MEM_Descripcion,
                    m.ESM_Estado_Movimiento,
                    em.ESM_Descripcion,
                    m.RCA_Regla_Recargo,
                    m.MOV_Monto_Origen,
                    CASE
                        WHEN UPPER(TRIM(mm.MEM_Descripcion)) = 'CARGO BANCARIO' THEN m.MOV_Monto
                        ELSE 0
                    END AS MOV_Recargo,
                    m.MOV_Monto,
                    m.MOV_Saldo,
                    m.MOV_Fecha,
                    m.MOV_Numero_Referencia,
                    m.MOV_Descripcion,
                    m.MOV_Fecha_Creacion
                FROM GCB_MOVIMIENTO m
                INNER JOIN GCB_CUENTA_BANCARIA cb
                    ON cb.CUB_Cuenta = m.CUB_Cuenta
                INNER JOIN GCB_TIPO_MOVIMIENTO tm
                    ON tm.TIM_Tipo_Movimiento = m.TIM_Tipo_Movimiento
                INNER JOIN GCB_MEDIO_MOVIMIENTO mm
                    ON mm.MEM_Medio_Movimiento = m.MEM_Medio_Movimiento
                INNER JOIN GCB_ESTADO_MOVIMIENTO em
                    ON em.ESM_Estado_Movimiento = m.ESM_Estado_Movimiento
                LEFT JOIN GCB_PERSONA p
                    ON p.PER_Persona = m.PER_Persona
                ORDER BY m.MOV_Fecha DESC, m.MOV_Movimiento DESC";

            return await connection.QueryAsync<ResponseMovimientoDTO>(sql);
        }

        public async Task<ResponseMovimientoDTO?> ObtenerPorId(int id)
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT
                    m.MOV_Movimiento,
                    m.CUB_Cuenta,
                    cb.CUB_Numero_Cuenta,
                    m.PER_Persona,
                    CASE
                        WHEN p.PER_Persona IS NULL THEN NULL
                        WHEN p.PER_Razon_Social IS NOT NULL THEN p.PER_Razon_Social
                        ELSE TRIM(
                            NVL(p.PER_Primer_Nombre, '') || ' ' ||
                            NVL(p.PER_Segundo_Nombre, '') || ' ' ||
                            NVL(p.PER_Primer_Apellido, '') || ' ' ||
                            NVL(p.PER_Segundo_Apellido, '')
                        )
                    END AS PER_Nombre_Completo,
                    m.TIM_Tipo_Movimiento,
                    tm.TIM_Descripcion,
                    m.MEM_Medio_Movimiento,
                    mm.MEM_Descripcion,
                    m.ESM_Estado_Movimiento,
                    em.ESM_Descripcion,
                    m.RCA_Regla_Recargo,
                    m.MOV_Monto_Origen,
                    CASE
                        WHEN UPPER(TRIM(mm.MEM_Descripcion)) = 'CARGO BANCARIO' THEN m.MOV_Monto
                        ELSE 0
                    END AS MOV_Recargo,
                    m.MOV_Monto,
                    m.MOV_Saldo,
                    m.MOV_Fecha,
                    m.MOV_Numero_Referencia,
                    m.MOV_Descripcion,
                    m.MOV_Fecha_Creacion
                FROM GCB_MOVIMIENTO m
                INNER JOIN GCB_CUENTA_BANCARIA cb
                    ON cb.CUB_Cuenta = m.CUB_Cuenta
                INNER JOIN GCB_TIPO_MOVIMIENTO tm
                    ON tm.TIM_Tipo_Movimiento = m.TIM_Tipo_Movimiento
                INNER JOIN GCB_MEDIO_MOVIMIENTO mm
                    ON mm.MEM_Medio_Movimiento = m.MEM_Medio_Movimiento
                INNER JOIN GCB_ESTADO_MOVIMIENTO em
                    ON em.ESM_Estado_Movimiento = m.ESM_Estado_Movimiento
                LEFT JOIN GCB_PERSONA p
                    ON p.PER_Persona = m.PER_Persona
                WHERE m.MOV_Movimiento = :Id";

            return await connection.QueryFirstOrDefaultAsync<ResponseMovimientoDTO>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ResponseMovimientoDTO>> ObtenerPorCuenta(int cuentaId)
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT
                    m.MOV_Movimiento,
                    m.CUB_Cuenta,
                    cb.CUB_Numero_Cuenta,
                    m.PER_Persona,
                    CASE
                        WHEN p.PER_Persona IS NULL THEN NULL
                        WHEN p.PER_Razon_Social IS NOT NULL THEN p.PER_Razon_Social
                        ELSE TRIM(
                            NVL(p.PER_Primer_Nombre, '') || ' ' ||
                            NVL(p.PER_Segundo_Nombre, '') || ' ' ||
                            NVL(p.PER_Primer_Apellido, '') || ' ' ||
                            NVL(p.PER_Segundo_Apellido, '')
                        )
                    END AS PER_Nombre_Completo,
                    m.TIM_Tipo_Movimiento,
                    tm.TIM_Descripcion,
                    m.MEM_Medio_Movimiento,
                    mm.MEM_Descripcion,
                    m.ESM_Estado_Movimiento,
                    em.ESM_Descripcion,
                    m.RCA_Regla_Recargo,
                    m.MOV_Monto_Origen,
                    CASE
                        WHEN UPPER(TRIM(mm.MEM_Descripcion)) = 'CARGO BANCARIO' THEN m.MOV_Monto
                        ELSE 0
                    END AS MOV_Recargo,
                    m.MOV_Monto,
                    m.MOV_Saldo,
                    m.MOV_Fecha,
                    m.MOV_Numero_Referencia,
                    m.MOV_Descripcion,
                    m.MOV_Fecha_Creacion
                FROM GCB_MOVIMIENTO m
                INNER JOIN GCB_CUENTA_BANCARIA cb
                    ON cb.CUB_Cuenta = m.CUB_Cuenta
                INNER JOIN GCB_TIPO_MOVIMIENTO tm
                    ON tm.TIM_Tipo_Movimiento = m.TIM_Tipo_Movimiento
                INNER JOIN GCB_MEDIO_MOVIMIENTO mm
                    ON mm.MEM_Medio_Movimiento = m.MEM_Medio_Movimiento
                INNER JOIN GCB_ESTADO_MOVIMIENTO em
                    ON em.ESM_Estado_Movimiento = m.ESM_Estado_Movimiento
                LEFT JOIN GCB_PERSONA p
                    ON p.PER_Persona = m.PER_Persona
                WHERE m.CUB_Cuenta = :CuentaId
                ORDER BY m.MOV_Fecha DESC, m.MOV_Movimiento DESC";

            return await connection.QueryAsync<ResponseMovimientoDTO>(sql, new { CuentaId = cuentaId });
        }

        public async Task AnularConRecargo(int movimientoId)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var trx = connection.BeginTransaction();

            try
            {
                var movimiento = await connection.QueryFirstOrDefaultAsync<dynamic>(@"
                    SELECT
                        MOV_Movimiento,
                        CUB_Cuenta,
                        TIM_Tipo_Movimiento,
                        ESM_Estado_Movimiento,
                        MOV_Monto,
                        MOV_Numero_Referencia
                    FROM GCB_MOVIMIENTO
                    WHERE MOV_Movimiento = :Id",
                    new { Id = movimientoId }, trx);

                if (movimiento == null)
                    throw new Exception("Movimiento no encontrado.");

                string? estadoActual = await connection.ExecuteScalarAsync<string>(@"
                    SELECT ESM_Descripcion
                    FROM GCB_ESTADO_MOVIMIENTO
                    WHERE ESM_Estado_Movimiento = :Id",
                    new { Id = (int)movimiento.ESM_Estado_Movimiento }, trx);

                if (string.Equals(estadoActual?.Trim(), "Anulado", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("El movimiento ya está anulado.");

                string? tipoDescripcion = await connection.ExecuteScalarAsync<string>(@"
                    SELECT TIM_Descripcion
                    FROM GCB_TIPO_MOVIMIENTO
                    WHERE TIM_Tipo_Movimiento = :Id",
                    new { Id = (int)movimiento.TIM_Tipo_Movimiento }, trx);

                int estadoAnuladoId = await ObtenerEstadoAnuladoId(connection, trx);

                decimal saldoActual = await connection.ExecuteScalarAsync<decimal>(@"
                    SELECT NVL(CUB_Saldo_Actual, 0)
                    FROM GCB_CUENTA_BANCARIA
                    WHERE CUB_Cuenta = :Id",
                    new { Id = (int)movimiento.CUB_Cuenta }, trx);

                var recargo = await connection.QueryFirstOrDefaultAsync<dynamic>(@"
                    SELECT
                        MOV_Movimiento,
                        MOV_Monto,
                        ESM_Estado_Movimiento
                    FROM GCB_MOVIMIENTO
                    WHERE CUB_Cuenta = :Cuenta
                      AND MOV_Numero_Referencia = :ReferenciaRecargo",
                    new
                    {
                        Cuenta = (int)movimiento.CUB_Cuenta,
                        ReferenciaRecargo = $"{(string)movimiento.MOV_Numero_Referencia}-RCG"
                    }, trx);

                decimal montoRecargo = 0m;

                if (recargo != null)
                {
                    string? estadoRecargo = await connection.ExecuteScalarAsync<string>(@"
                        SELECT ESM_Descripcion
                        FROM GCB_ESTADO_MOVIMIENTO
                        WHERE ESM_Estado_Movimiento = :Id",
                        new { Id = (int)recargo.ESM_Estado_Movimiento }, trx);

                    if (!string.Equals(estadoRecargo?.Trim(), "Anulado", StringComparison.OrdinalIgnoreCase))
                    {
                        montoRecargo = (decimal)recargo.MOV_Monto;

                        await connection.ExecuteAsync(@"
                            UPDATE GCB_MOVIMIENTO
                            SET ESM_Estado_Movimiento = :Estado
                            WHERE MOV_Movimiento = :Id",
                            new
                            {
                                Estado = estadoAnuladoId,
                                Id = (int)recargo.MOV_Movimiento
                            }, trx);
                    }
                }

                await connection.ExecuteAsync(@"
                    UPDATE GCB_MOVIMIENTO
                    SET ESM_Estado_Movimiento = :Estado
                    WHERE MOV_Movimiento = :Id",
                    new
                    {
                        Estado = estadoAnuladoId,
                        Id = movimientoId
                    }, trx);

                decimal saldoRevertido;

                if (string.Equals(tipoDescripcion?.Trim(), "Ingreso", StringComparison.OrdinalIgnoreCase))
                {
                    if (saldoActual < (decimal)movimiento.MOV_Monto)
                        throw new Exception("No se puede anular el movimiento porque el saldo actual no lo permite.");

                    saldoRevertido = saldoActual - (decimal)movimiento.MOV_Monto;
                }
                else
                {
                    saldoRevertido = saldoActual + (decimal)movimiento.MOV_Monto + montoRecargo;
                }

                await connection.ExecuteAsync(@"
                    UPDATE GCB_CUENTA_BANCARIA
                    SET CUB_Saldo_Actual = :Saldo
                    WHERE CUB_Cuenta = :Cuenta",
                    new
                    {
                        Cuenta = (int)movimiento.CUB_Cuenta,
                        Saldo = saldoRevertido
                    }, trx);

                trx.Commit();
            }
            catch
            {
                trx.Rollback();
                throw;
            }
        }

        public async Task<bool> ExisteCuentaActiva(int cuentaId)
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM GCB_CUENTA_BANCARIA
                WHERE CUB_Cuenta = :Id
                  AND CUB_Estado = 'A'";

            return await connection.ExecuteScalarAsync<int>(sql, new { Id = cuentaId }) > 0;
        }

        public async Task<bool> ExistePersonaActiva(int personaId)
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM GCB_PERSONA
                WHERE PER_Persona = :Id
                  AND PER_Estado = 'A'";

            return await connection.ExecuteScalarAsync<int>(sql, new { Id = personaId }) > 0;
        }

        public async Task<bool> ExisteTipoMovimientoActivo(int tipoId)
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM GCB_TIPO_MOVIMIENTO
                WHERE TIM_Tipo_Movimiento = :Id
                  AND TIM_Estado = 'A'";

            return await connection.ExecuteScalarAsync<int>(sql, new { Id = tipoId }) > 0;
        }

        public async Task<bool> ExisteMedioMovimientoActivo(int medioId)
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM GCB_MEDIO_MOVIMIENTO
                WHERE MEM_Medio_Movimiento = :Id
                  AND MEM_Estado = 'A'";

            return await connection.ExecuteScalarAsync<int>(sql, new { Id = medioId }) > 0;
        }

        public async Task<bool> ExisteEstadoMovimientoActivo(int estadoId)
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM GCB_ESTADO_MOVIMIENTO
                WHERE ESM_Estado_Movimiento = :Id
                  AND ESM_Estado = 'A'";

            return await connection.ExecuteScalarAsync<int>(sql, new { Id = estadoId }) > 0;
        }

        private async Task<int> InsertarMovimiento(
            OracleConnection connection,
            OracleTransaction trx,
            int cuentaId,
            int? personaId,
            int tipoMovimientoId,
            int medioMovimientoId,
            int estadoMovimientoId,
            DateTime fecha,
            string? numeroReferencia,
            string descripcion,
            decimal monto,
            decimal montoOrigen,
            decimal saldo)
        {
            string sql = @"
                INSERT INTO GCB_MOVIMIENTO
                (
                    CUB_Cuenta,
                    PER_Persona,
                    TIM_Tipo_Movimiento,
                    MEM_Medio_Movimiento,
                    ESM_Estado_Movimiento,
                    RCA_Regla_Recargo,
                    MOV_Fecha,
                    MOV_Numero_Referencia,
                    MOV_Descripcion,
                    MOV_Monto,
                    MOV_Monto_Origen,
                    MOV_Saldo,
                    MOV_Fecha_Creacion
                )
                VALUES
                (
                    :CUB_Cuenta,
                    :PER_Persona,
                    :TIM_Tipo_Movimiento,
                    :MEM_Medio_Movimiento,
                    :ESM_Estado_Movimiento,
                    NULL,
                    :MOV_Fecha,
                    :MOV_Numero_Referencia,
                    :MOV_Descripcion,
                    :MOV_Monto,
                    :MOV_Monto_Origen,
                    :MOV_Saldo,
                    SYSDATE
                )
                RETURNING MOV_Movimiento INTO :Id";

            var parameters = new DynamicParameters();
            parameters.Add("CUB_Cuenta", cuentaId);
            parameters.Add("PER_Persona", personaId);
            parameters.Add("TIM_Tipo_Movimiento", tipoMovimientoId);
            parameters.Add("MEM_Medio_Movimiento", medioMovimientoId);
            parameters.Add("ESM_Estado_Movimiento", estadoMovimientoId);
            parameters.Add("MOV_Fecha", fecha);
            parameters.Add("MOV_Numero_Referencia", numeroReferencia);
            parameters.Add("MOV_Descripcion", descripcion);
            parameters.Add("MOV_Monto", monto);
            parameters.Add("MOV_Monto_Origen", montoOrigen);
            parameters.Add("MOV_Saldo", saldo);
            parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(sql, parameters, trx);

            return parameters.Get<int>("Id");
        }

        private async Task<int> ObtenerMedioCargoBancarioId(OracleConnection connection, OracleTransaction trx)
        {
            var id = await connection.QueryFirstOrDefaultAsync<int?>(@"
                SELECT MEM_Medio_Movimiento
                FROM GCB_MEDIO_MOVIMIENTO
                WHERE UPPER(TRIM(MEM_Descripcion)) = 'CARGO BANCARIO'
                  AND MEM_Estado = 'A'
                FETCH FIRST 1 ROWS ONLY", transaction: trx);

            if (!id.HasValue)
                throw new Exception("Debes crear en GCB_MEDIO_MOVIMIENTO el medio 'Cargo bancario'.");

            return id.Value;
        }

        private async Task<int> ObtenerEstadoAnuladoId(OracleConnection connection, OracleTransaction trx)
        {
            var id = await connection.QueryFirstOrDefaultAsync<int?>(@"
                SELECT ESM_Estado_Movimiento
                FROM GCB_ESTADO_MOVIMIENTO
                WHERE UPPER(TRIM(ESM_Descripcion)) = 'ANULADO'
                  AND ESM_Estado = 'A'
                FETCH FIRST 1 ROWS ONLY", transaction: trx);

            if (!id.HasValue)
                throw new Exception("Debes tener el estado 'Anulado' activo en GCB_ESTADO_MOVIMIENTO.");

            return id.Value;
        }

        private string GenerarReferencia(string? referencia)
        {
            if (!string.IsNullOrWhiteSpace(referencia))
                return referencia.Trim();

            return $"MOV-{DateTime.Now:yyyyMMddHHmmssfff}";
        }
    }
}