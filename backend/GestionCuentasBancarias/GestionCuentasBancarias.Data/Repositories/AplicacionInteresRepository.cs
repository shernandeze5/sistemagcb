using Dapper;
using GestionCuentasBancarias.Domain.DTOS.AplicacionInteres;
using GestionCuentasBancarias.Domain.DTOS.TasaInteres;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text.RegularExpressions;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class AplicacionInteresRepository : IAplicacionInteresRepository
    {
        private readonly string connectionString;

        public AplicacionInteresRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<int> AplicarInteres(CreateAplicacionInteresDTO dto)
        {
            using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 🔥 0. VALIDACIONES
                if (dto.TIN_Tasa_Interes <= 0)
                    throw new Exception("Tasa inválida");

                if (!Regex.IsMatch(dto.Periodo, @"^\d{4}-\d{2}$"))
                    throw new Exception("Formato de período inválido. Use YYYY-MM");

                // 🔥 1. OBTENER TASA ACTIVA
                string tasaSql = @"
                    SELECT TIN_Tasa_Interes, CUB_Cuenta, TIN_Porcentaje
                    FROM GCB_TASA_INTERES
                    WHERE TIN_Tasa_Interes = :Id AND TIN_Estado = 'A'";

                var tasa = await connection.QueryFirstOrDefaultAsync<TasaInteresDTO>(
                    tasaSql,
                    new { Id = dto.TIN_Tasa_Interes },
                    transaction
                );

                if (tasa == null)
                    throw new Exception("Tasa no encontrada o inactiva");

                // 🔥 2. OBTENER SALDO ACTUAL
                string saldoSql = @"
                    SELECT CUB_Saldo_Actual 
                    FROM GCB_CUENTA_BANCARIA 
                    WHERE CUB_Cuenta = :Id";

                decimal saldoActual = await connection.ExecuteScalarAsync<decimal>(
                    saldoSql,
                    new { Id = tasa.CUB_Cuenta },
                    transaction
                );

                // 🔥 3. VALIDAR PERIODO DUPLICADO
                string checkPeriodo = @"
                    SELECT COUNT(1)
                    FROM GCB_APLICACION_INTERES
                    WHERE TIN_Tasa_Interes = :Tasa 
                    AND AIN_Periodo = :Periodo";

                int existe = await connection.ExecuteScalarAsync<int>(
                    checkPeriodo,
                    new { Tasa = dto.TIN_Tasa_Interes, Periodo = dto.Periodo },
                    transaction
                );

                if (existe > 0)
                    throw new Exception("Interés ya aplicado en este período");

                // 🔥 4. CALCULAR INTERÉS
                decimal montoInteres = Math.Round(
                    saldoActual * (tasa.TIN_Porcentaje / 100),
                    2
                );

                decimal nuevoSaldo = saldoActual + montoInteres;

                // 🔥 5. INSERTAR MOVIMIENTO (CORREGIDO)
                string insertMovimiento = @"
                INSERT INTO GCB_MOVIMIENTO
                (CUB_Cuenta, TIM_Tipo_Movimiento, MEM_Medio_MOVIMIENTO, MOV_Fecha,
                 MOV_Numero_Referencia, MOV_Descripcion, MOV_Monto, MOV_Saldo, ESM_Estado_Movimiento)
                VALUES (:Cuenta, 1, 1, SYSDATE,
                        :Referencia, 'Acreditamiento de Interés', :Monto, :Saldo, 1)
                RETURNING MOV_Movimiento INTO :Id";

                var parameters = new DynamicParameters();
                parameters.Add("Cuenta", tasa.CUB_Cuenta);
                parameters.Add("Referencia", $"INT-{dto.Periodo}");
                parameters.Add("Monto", montoInteres);
                parameters.Add("Saldo", nuevoSaldo);
                parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(insertMovimiento, parameters, transaction);

                int movimientoId = parameters.Get<int>("Id");

                // 🔥 6. INSERTAR APLICACIÓN INTERÉS
                string insertAplicacion = @"
                    INSERT INTO GCB_APLICACION_INTERES
                    (TIN_Tasa_Interes, MOV_Movimiento, AIN_Saldo_Base, AIN_Monto_Calculado, AIN_Periodo, AIN_Fecha_Aplicacion)
                    VALUES (:Tasa, :Movimiento, :SaldoBase, :MontoCalculado, :Periodo, SYSDATE)";

                await connection.ExecuteAsync(insertAplicacion, new
                {
                    Tasa = dto.TIN_Tasa_Interes,
                    Movimiento = movimientoId,
                    SaldoBase = saldoActual,
                    MontoCalculado = montoInteres,
                    Periodo = dto.Periodo
                }, transaction);

                // 🔥 7. ACTUALIZAR SALDO CUENTA
                string updateCuenta = @"
                    UPDATE GCB_CUENTA_BANCARIA
                    SET CUB_Saldo_Actual = :Saldo
                    WHERE CUB_Cuenta = :Cuenta";

                await connection.ExecuteAsync(updateCuenta, new
                {
                    Saldo = nuevoSaldo,
                    Cuenta = tasa.CUB_Cuenta
                }, transaction);

                transaction.Commit();
                return movimientoId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<ResponseAplicacionInteresDTO>> ObtenerPorCuenta(int cuentaId)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
                SELECT 
                    ai.AIN_Aplicacion_Interes,
                    ai.TIN_Tasa_Interes,
                    ai.MOV_Movimiento,
                    ai.AIN_Saldo_Base,
                    ai.AIN_Monto_Calculado,
                    ai.AIN_Periodo,
                    ai.AIN_Fecha_Aplicacion
                FROM GCB_APLICACION_INTERES ai
                INNER JOIN GCB_TASA_INTERES t 
                    ON ai.TIN_Tasa_Interes = t.TIN_Tasa_Interes
                WHERE t.CUB_Cuenta = :Cuenta
                ORDER BY ai.AIN_Fecha_Aplicacion DESC";

            return await connection.QueryAsync<ResponseAplicacionInteresDTO>(
                sql,
                new { Cuenta = cuentaId }
            );
        }

        public async Task<IEnumerable<ResponseAplicacionInteresDTO>> ObtenerPorTasa(int tasaId)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
                SELECT 
                    AIN_Aplicacion_Interes,
                    TIN_Tasa_Interes,
                    MOV_Movimiento,
                    AIN_Saldo_Base,
                    AIN_Monto_Calculado,
                    AIN_Periodo,
                    AIN_Fecha_Aplicacion
                FROM GCB_APLICACION_INTERES
                WHERE TIN_Tasa_Interes = :Tasa
                ORDER BY AIN_Fecha_Aplicacion DESC";

            return await connection.QueryAsync<ResponseAplicacionInteresDTO>(
                sql,
                new { Tasa = tasaId }
            );
        }

        public async Task<IEnumerable<TasaInteresFrecuenciaDTO>> ObtenerTasasActivas()
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
             SELECT 
                t.TIN_Tasa_Interes,
                t.CUB_Cuenta,
                t.TIN_Porcentaje,
                f.INF_Descripcion AS Frecuencia
                    FROM GCB_TASA_INTERES t
                    INNER JOIN GCB_INTERES_FRECUENCIA f
                        ON t.INF_Frecuencia = f.INF_Interes_Frecuencia
                    WHERE t.TIN_Estado = 'A'";

            return await connection.QueryAsync<TasaInteresFrecuenciaDTO>(sql);
        }


        public async Task AplicarRecargoAutomatico(int cuentaId)
        {
            using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();
            using var trx = connection.BeginTransaction();

            try
            {
                // 1. obtener regla
                string reglaSql = @"
            SELECT * FROM GCB_REGLA_RECARGO
            WHERE CUB_Cuenta = :Cuenta AND RCA_Estado = 'A'";

                var regla = await connection.QueryFirstOrDefaultAsync(reglaSql,
                    new { Cuenta = cuentaId }, trx);

                if (regla == null)
                    return;

                // 2. saldo actual
                string saldoSql = @"SELECT CUB_Saldo_Actual FROM GCB_CUENTA_BANCARIA WHERE CUB_Cuenta = :Id";

                decimal saldo = await connection.ExecuteScalarAsync<decimal>(saldoSql,
                    new { Id = cuentaId }, trx);

                decimal nuevoSaldo = saldo - regla.RCA_Monto;

                // 3. insertar movimiento
                string sql = @"
            INSERT INTO GCB_MOVIMIENTO
            (CUB_Cuenta, TIM_Tipo_Movimiento, MEM_Medio_MOVIMIENTO, MOV_Fecha,
             MOV_Descripcion, MOV_Monto, MOV_Saldo, ESM_Estado_Movimiento)
            VALUES (:Cuenta, 2, 1, SYSDATE,
                    'Recargo automático', :Monto, :Saldo, 1)";

                await connection.ExecuteAsync(sql, new
                {
                    Cuenta = cuentaId,
                    Monto = -regla.RCA_Monto,
                    Saldo = nuevoSaldo
                }, trx);

                // 4. actualizar saldo
                await connection.ExecuteAsync(
                    @"UPDATE GCB_CUENTA_BANCARIA SET CUB_Saldo_Actual = :Saldo WHERE CUB_Cuenta = :Cuenta",
                    new { Saldo = nuevoSaldo, Cuenta = cuentaId }, trx);

                trx.Commit();
            }
            catch
            {
                trx.Rollback();
                throw;
            }
        }
    }
}