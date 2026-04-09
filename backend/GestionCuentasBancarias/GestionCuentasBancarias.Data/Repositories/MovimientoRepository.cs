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
            connectionString = configuration.GetConnectionString("OracleConnection")
                ?? throw new Exception("Connection string no configurada");
        }

        // ===============================
        // 📄 OBTENER MOVIMIENTOS POR CUENTA
        // ===============================
        public async Task<List<ResponseMovimientoDTO>> ObtenerPorCuenta(int cuentaId)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
                SELECT 
                    m.MOV_Movimiento,
                    m.CUB_Cuenta,
                    m.PER_Persona,
                    m.MOV_Monto,
                    m.MOV_Saldo,
                    tm.TIM_Descripcion AS TipoMovimiento,
                    mm.MEM_Descripcion AS MedioMovimiento,
                    em.ESM_Descripcion AS EstadoMovimiento,
                    m.MOV_Fecha,
                    m.MOV_Descripcion,
                    m.MOV_Numero_Referencia
                FROM GCB_MOVIMIENTO m
                INNER JOIN GCB_TIPO_MOVIMIENTO tm 
                    ON m.TIM_Tipo_Movimiento = tm.TIM_Tipo_Movimiento
                INNER JOIN GCB_MEDIO_MOVIMIENTO mm 
                    ON m.MEM_Medio_Movimiento = mm.MEM_Medio_Movimiento
                INNER JOIN GCB_ESTADO_MOVIMIENTO em 
                    ON m.ESM_Estado_Movimiento = em.ESM_Estado_Movimiento
                WHERE m.CUB_Cuenta = :Cuenta
                ORDER BY m.MOV_Fecha DESC";

            var result = await connection.QueryAsync<ResponseMovimientoDTO>(sql, new { Cuenta = cuentaId });

            return result.ToList();
        }

        // ===============================
        // 🔍 OBTENER MOVIMIENTO POR ID
        // ===============================
        public async Task<ResponseMovimientoDTO?> ObtenerPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
                SELECT 
                    MOV_Movimiento,
                    CUB_Cuenta,
                    PER_Persona,
                    MOV_Monto,
                    MOV_Saldo,
                    MOV_Fecha,
                    MOV_Descripcion,
                    MOV_Numero_Referencia,
                    TIM_Tipo_Movimiento,
                    MEM_Medio_Movimiento,
                    ESM_Estado_Movimiento
                FROM GCB_MOVIMIENTO
                WHERE MOV_Movimiento = :Id";

            return await connection.QueryFirstOrDefaultAsync<ResponseMovimientoDTO>(sql, new { Id = id });
        }

        // ===============================
        // 💾 CREAR MOVIMIENTO (CORRECTO)
        // ===============================
        public async Task<int> CrearMovimiento(CreateMovimientoDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
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
                    MOV_Saldo,
                    ESM_Estado_Movimiento
                )
                VALUES
                (
                    :Cuenta,
                    :Persona,
                    :Tipo,
                    :Medio,
                    :Fecha,
                    :Referencia,
                    :Descripcion,
                    :Monto,
                    :Saldo,
                    :Estado
                )
                RETURNING MOV_Movimiento INTO :Id";

            var parameters = new DynamicParameters();

            parameters.Add("Cuenta", dto.CUB_Cuenta);
            parameters.Add("Persona", dto.PER_Persona == 0 ? null : dto.PER_Persona);
            parameters.Add("Tipo", dto.TIM_Tipo_Movimiento);
            parameters.Add("Medio", dto.MEM_Medio_Movimiento);
            parameters.Add("Fecha", dto.MOV_Fecha);
            parameters.Add("Referencia", dto.MOV_Numero_Referencia);
            parameters.Add("Descripcion", dto.MOV_Descripcion);
            parameters.Add("Monto", dto.MOV_Monto);
            parameters.Add("Saldo", dto.MOV_Saldo); // 🔥 YA CALCULADO EN SERVICE
            parameters.Add("Estado", dto.ESM_Estado_Movimiento);

            // 🔥 OUTPUT PARAM (CORRECTO)
            parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(sql, parameters);

            return parameters.Get<int>("Id");
        }
    }
}