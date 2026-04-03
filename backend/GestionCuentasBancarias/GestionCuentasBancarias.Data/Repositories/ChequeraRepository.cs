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
    public class ChequeraRepository : IChequeraRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public ChequeraRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Chequera>> ObtenerTodosAsync()
        {
            var sql = @"
                SELECT 
                    CHQ_Chequera,
                    CUB_Cuenta,
                    CHQ_Serie,
                    CHQ_Numero_Desde,
                    CHQ_Numero_Hasta,
                    CHQ_Ultimo_Usado,
                    CHQ_Estado,
                    CHQ_Fecha_Recepcion,
                    CHQ_Fecha_Creacion
                FROM GCB_CHEQUERA
                ORDER BY CHQ_Chequera";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Chequera>(sql);
        }

        public async Task<Chequera?> ObtenerPorIdAsync(int id)
        {
            var sql = @"
                SELECT 
                    CHQ_Chequera,
                    CUB_Cuenta,
                    CHQ_Serie,
                    CHQ_Numero_Desde,
                    CHQ_Numero_Hasta,
                    CHQ_Ultimo_Usado,
                    CHQ_Estado,
                    CHQ_Fecha_Recepcion,
                    CHQ_Fecha_Creacion
                FROM GCB_CHEQUERA
                WHERE CHQ_Chequera = :Id";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Chequera>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(Chequera chequera)
        {
            var sql = @"
                INSERT INTO GCB_CHEQUERA
                (
                    CUB_Cuenta,
                    CHQ_Serie,
                    CHQ_Numero_Desde,
                    CHQ_Numero_Hasta,
                    CHQ_Ultimo_Usado,
                    CHQ_Estado,
                    CHQ_Fecha_Recepcion,
                    CHQ_Fecha_Creacion
                )
                VALUES
                (
                    :CUB_Cuenta,
                    :CHQ_Serie,
                    :CHQ_Numero_Desde,
                    :CHQ_Numero_Hasta,
                    :CHQ_Ultimo_Usado,
                    :CHQ_Estado,
                    :CHQ_Fecha_Recepcion,
                    :CHQ_Fecha_Creacion
                )";

            using var connection = _connectionFactory.CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(sql, chequera);
            return filasAfectadas > 0;
        }

        public async Task<bool> ActualizarAsync(Chequera chequera)
        {
            var sql = @"
                UPDATE GCB_CHEQUERA
                SET
                    CUB_Cuenta = :CUB_Cuenta,
                    CHQ_Serie = :CHQ_Serie,
                    CHQ_Numero_Desde = :CHQ_Numero_Desde,
                    CHQ_Numero_Hasta = :CHQ_Numero_Hasta,
                    CHQ_Ultimo_Usado = :CHQ_Ultimo_Usado,
                    CHQ_Estado = :CHQ_Estado,
                    CHQ_Fecha_Recepcion = :CHQ_Fecha_Recepcion
                WHERE CHQ_Chequera = :CHQ_Chequera";

            using var connection = _connectionFactory.CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(sql, chequera);
            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            var sql = @"
                UPDATE GCB_CHEQUERA
                SET CHQ_Estado = 'Anulada'
                WHERE CHQ_Chequera = :Id";

            using var connection = _connectionFactory.CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(sql, new { Id = id });
            return filasAfectadas > 0;
        }

        public async Task<bool> Reactivar(int id)
        {
            var sql = @"
                UPDATE GCB_CHEQUERA
                SET CHQ_Estado = 'Pendiente'
                WHERE CHQ_Chequera = :Id";

            using var connection = _connectionFactory.CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(sql, new { Id = id });
            return filasAfectadas > 0;
        }
    }
}
