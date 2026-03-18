using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public MovimientoRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Movimiento>> ObtenerTodosAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    MOV_Movimiento,
                    CUB_Cuenta,
                    PER_Persona,
                    TIM_Tipo_Movimiento,
                    MEM_Medio_Movimiento,
                    ESM_Estado_Movimiento,
                    MOV_Fecha,
                    MOV_Numero_Referencia,
                    MOV_Descripcion,
                    MOV_Monto,
                    MOV_Saldo
                FROM GCB_MOVIMIENTO
                ORDER BY MOV_Movimiento";

            return await connection.QueryAsync<Movimiento>(sql);
        }

        public async Task<Movimiento?> ObtenerPorIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    MOV_Movimiento,
                    CUB_Cuenta,
                    PER_Persona,
                    TIM_Tipo_Movimiento,
                    MEM_Medio_Movimiento,
                    ESM_Estado_Movimiento,
                    MOV_Fecha,
                    MOV_Numero_Referencia,
                    MOV_Descripcion,
                    MOV_Monto,
                    MOV_Saldo
                FROM GCB_MOVIMIENTO
                WHERE MOV_Movimiento = :Id";

            return await connection.QueryFirstOrDefaultAsync<Movimiento>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(Movimiento movimiento)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                INSERT INTO GCB_MOVIMIENTO
                (
                    CUB_Cuenta,
                    PER_Persona,
                    TIM_Tipo_Movimiento,
                    MEM_Medio_Movimiento,
                    ESM_Estado_Movimiento,
                    MOV_Fecha,
                    MOV_Numero_Referencia,
                    MOV_Descripcion,
                    MOV_Monto,
                    MOV_Saldo
                )
                VALUES
                (
                    :CUB_Cuenta,
                    :PER_Persona,
                    :TIM_Tipo_Movimiento,
                    :MEM_Medio_Movimiento,
                    :ESM_Estado_Movimiento,
                    :MOV_Fecha,
                    :MOV_Numero_Referencia,
                    :MOV_Descripcion,
                    :MOV_Monto,
                    :MOV_Saldo
                )";

            var resultado = await connection.ExecuteAsync(sql, new
            {
                movimiento.CUB_Cuenta,
                movimiento.PER_Persona,
                movimiento.TIM_Tipo_Movimiento,
                movimiento.MEM_Medio_Movimiento,
                movimiento.ESM_Estado_Movimiento,
                movimiento.MOV_Fecha,
                movimiento.MOV_Numero_Referencia,
                movimiento.MOV_Descripcion,
                movimiento.MOV_Monto,
                movimiento.MOV_Saldo
            });

            return resultado > 0;
        }

        public async Task<bool> ActualizarAsync(Movimiento movimiento)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                UPDATE GCB_MOVIMIENTO
                SET
                    MOV_Descripcion = :MOV_Descripcion,
                    ESM_Estado_Movimiento = :ESM_Estado_Movimiento
                WHERE MOV_Movimiento = :MOV_Movimiento";

            var resultado = await connection.ExecuteAsync(sql, new
            {
                movimiento.MOV_Movimiento,
                movimiento.MOV_Descripcion,
                movimiento.ESM_Estado_Movimiento
            });

            return resultado > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                UPDATE GCB_MOVIMIENTO
                SET ESM_Estado_Movimiento = 0
                WHERE MOV_Movimiento = :Id";

            var resultado = await connection.ExecuteAsync(sql, new { Id = id });

            return resultado > 0;
        }
    }
}
