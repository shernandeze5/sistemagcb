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
    public class TipoMovimientoRepository : ITipoMovimientoRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public TipoMovimientoRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TipoMovimiento>> ObtenerTodosAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    TIM_Tipo_Movimiento,
                    TIM_Descripcion,
                    TIM_Estado,
                    TIM_Fecha_Creacion
                FROM GCB_TIPO_MOVIMIENTO
                WHERE TIM_Estado = 1
                ORDER BY TIM_Tipo_Movimiento";

            return await connection.QueryAsync<TipoMovimiento>(sql);
        }

        public async Task<TipoMovimiento?> ObtenerPorIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    TIM_Tipo_Movimiento,
                    TIM_Descripcion,
                    TIM_Estado,
                    TIM_Fecha_Creacion
                FROM GCB_TIPO_MOVIMIENTO
                WHERE TIM_Tipo_Movimiento = :Id";

            return await connection.QueryFirstOrDefaultAsync<TipoMovimiento>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(TipoMovimiento tipoMovimiento)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                INSERT INTO GCB_TIPO_MOVIMIENTO
                (
                    TIM_Tipo_Movimiento,
                    TIM_Descripcion,
                    TIM_Estado,
                    TIM_Fecha_Creacion
                )
                VALUES
                (
                    SEQ_GCB_TIPO_MOVIMIENTO.NEXTVAL,
                    :TIM_Descripcion,
                    :TIM_Estado,
                    :TIM_Fecha_Creacion
                )";

            var resultado = await connection.ExecuteAsync(sql, new
            {
                tipoMovimiento.TIM_Descripcion,
                tipoMovimiento.TIM_Estado,
                tipoMovimiento.TIM_Fecha_Creacion
            });

            return resultado > 0;
        }

        public async Task<bool> ActualizarAsync(TipoMovimiento tipoMovimiento)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                UPDATE GCB_TIPO_MOVIMIENTO
                SET
                    TIM_Descripcion = :TIM_Descripcion,
                    TIM_Estado = :TIM_Estado
                WHERE TIM_Tipo_Movimiento = :TIM_Tipo_Movimiento";

            var resultado = await connection.ExecuteAsync(sql, new
            {
                tipoMovimiento.TIM_Tipo_Movimiento,
                tipoMovimiento.TIM_Descripcion,
                tipoMovimiento.TIM_Estado
            });

            return resultado > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                UPDATE GCB_TIPO_MOVIMIENTO
                SET TIM_Estado = 0
                WHERE TIM_Tipo_Movimiento = :Id";

            var resultado = await connection.ExecuteAsync(sql, new { Id = id });

            return resultado > 0;
        }
    }
}