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
    public class MedioMovimientoRepository : IMedioMovimientoRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public MedioMovimientoRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<MedioMovimiento>> ObtenerTodosAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    MEM_Medio_Movimiento,
                    MEM_Descripcion,
                    MEM_Estado,
                    MEM_Fecha_Creacion
                FROM GCB_MEDIO_MOVIMIENTO
                WHERE MEM_Estado = 'A'
                ORDER BY MEM_Medio_Movimiento";

            return await connection.QueryAsync<MedioMovimiento>(sql);
        }

        public async Task<MedioMovimiento?> ObtenerPorIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    MEM_Medio_Movimiento,
                    MEM_Descripcion,
                    MEM_Estado,
                    MEM_Fecha_Creacion
                FROM GCB_MEDIO_MOVIMIENTO
                WHERE MEM_Medio_Movimiento = :Id";

            return await connection.QueryFirstOrDefaultAsync<MedioMovimiento>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(MedioMovimiento medioMovimiento)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                INSERT INTO GCB_MEDIO_MOVIMIENTO
                (
                    MEM_Descripcion,
                    MEM_Estado,
                    MEM_Fecha_Creacion
                )
                VALUES
                (
                    :MEM_Descripcion,
                    :MEM_Estado,
                    :MEM_Fecha_Creacion
                )";

            var resultado = await connection.ExecuteAsync(sql, new
            {
                medioMovimiento.MEM_Descripcion,
                medioMovimiento.MEM_Estado,
                medioMovimiento.MEM_Fecha_Creacion
            });

            return resultado > 0;
        }

        public async Task<bool> ActualizarAsync(MedioMovimiento medioMovimiento)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                UPDATE GCB_MEDIO_MOVIMIENTO
                SET
                    MEM_Descripcion = :MEM_Descripcion,
                    MEM_Estado = :MEM_Estado
                WHERE MEM_Medio_Movimiento = :MEM_Medio_Movimiento";

            var resultado = await connection.ExecuteAsync(sql, new
            {
                medioMovimiento.MEM_Medio_Movimiento,
                medioMovimiento.MEM_Descripcion,
                medioMovimiento.MEM_Estado
            });

            return resultado > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                UPDATE GCB_MEDIO_MOVIMIENTO
                SET MEM_Estado = 'I'
                WHERE MEM_Medio_Movimiento = :Id";

            var resultado = await connection.ExecuteAsync(sql, new { Id = id });

            return resultado > 0;
        }
    }
}