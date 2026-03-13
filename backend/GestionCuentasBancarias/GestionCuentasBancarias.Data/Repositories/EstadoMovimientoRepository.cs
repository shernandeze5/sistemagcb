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
    public class EstadoMovimientoRepository : IEstadoMovimientoRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public EstadoMovimientoRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<EstadoMovimiento>> ObtenerTodosAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    ESM_Estado_Movimiento,
                    ESM_Descripcion,
                    ESM_Estado,
                    ESM_Fecha_Creacion
                FROM GCB_ESTADO_MOVIMIENTO
                WHERE ESM_Estado = 1
                ORDER BY ESM_Estado_Movimiento";

            return await connection.QueryAsync<EstadoMovimiento>(sql);
        }

        public async Task<EstadoMovimiento?> ObtenerPorIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    ESM_Estado_Movimiento,
                    ESM_Descripcion,
                    ESM_Estado,
                    ESM_Fecha_Creacion
                FROM GCB_ESTADO_MOVIMIENTO
                WHERE ESM_Estado_Movimiento = :Id";

            return await connection.QueryFirstOrDefaultAsync<EstadoMovimiento>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(EstadoMovimiento estadoMovimiento)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                INSERT INTO GCB_ESTADO_MOVIMIENTO
                (
                    ESM_Estado_Movimiento,
                    ESM_Descripcion,
                    ESM_Estado,
                    ESM_Fecha_Creacion
                )
                VALUES
                (
                    SEQ_GCB_ESTADO_MOVIMIENTO.NEXTVAL,
                    :ESM_Descripcion,
                    :ESM_Estado,
                    :ESM_Fecha_Creacion
                )";

            var resultado = await connection.ExecuteAsync(sql, new
            {
                estadoMovimiento.ESM_Descripcion,
                estadoMovimiento.ESM_Estado,
                estadoMovimiento.ESM_Fecha_Creacion
            });

            return resultado > 0;
        }

        public async Task<bool> ActualizarAsync(EstadoMovimiento estadoMovimiento)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                UPDATE GCB_ESTADO_MOVIMIENTO
                SET
                    ESM_Descripcion = :ESM_Descripcion,
                    ESM_Estado = :ESM_Estado
                WHERE ESM_Estado_Movimiento = :ESM_Estado_Movimiento";

            var resultado = await connection.ExecuteAsync(sql, new
            {
                estadoMovimiento.ESM_Estado_Movimiento,
                estadoMovimiento.ESM_Descripcion,
                estadoMovimiento.ESM_Estado
            });

            return resultado > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                UPDATE GCB_ESTADO_MOVIMIENTO
                SET ESM_Estado = 0
                WHERE ESM_Estado_Movimiento = :Id";

            var resultado = await connection.ExecuteAsync(sql, new { Id = id });

            return resultado > 0;
        }
    }
}