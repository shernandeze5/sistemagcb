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
                ORDER BY CHQ_Chequera DESC";

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
            ValidarChequera(chequera);

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

            var existeSerie = await connection.ExecuteScalarAsync<int>(@"
                SELECT COUNT(1)
                FROM GCB_CHEQUERA
                WHERE UPPER(TRIM(CHQ_Serie)) = UPPER(TRIM(:CHQ_Serie))
                  AND CUB_Cuenta = :CUB_Cuenta",
                new
                {
                    chequera.CHQ_Serie,
                    chequera.CUB_Cuenta
                });

            if (existeSerie > 0)
                throw new InvalidOperationException("Ya existe una chequera con esa serie para la cuenta.");

            var filasAfectadas = await connection.ExecuteAsync(sql, new
            {
                chequera.CUB_Cuenta,
                chequera.CHQ_Serie,
                chequera.CHQ_Numero_Desde,
                chequera.CHQ_Numero_Hasta,
                chequera.CHQ_Ultimo_Usado,
                CHQ_Estado = string.IsNullOrWhiteSpace(chequera.CHQ_Estado) ? "A" : chequera.CHQ_Estado,
                chequera.CHQ_Fecha_Recepcion,
                chequera.CHQ_Fecha_Creacion
            });

            return filasAfectadas > 0;
        }

        public async Task<bool> ActualizarAsync(Chequera chequera)
        {
            if (chequera.CHQ_Chequera <= 0)
                throw new InvalidOperationException("El id de la chequera es inválido.");

            ValidarChequera(chequera);

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

            var existeSerie = await connection.ExecuteScalarAsync<int>(@"
                SELECT COUNT(1)
                FROM GCB_CHEQUERA
                WHERE UPPER(TRIM(CHQ_Serie)) = UPPER(TRIM(:CHQ_Serie))
                  AND CUB_Cuenta = :CUB_Cuenta
                  AND CHQ_Chequera <> :CHQ_Chequera",
                new
                {
                    chequera.CHQ_Serie,
                    chequera.CUB_Cuenta,
                    chequera.CHQ_Chequera
                });

            if (existeSerie > 0)
                throw new InvalidOperationException("Ya existe otra chequera con esa serie para la cuenta.");

            var filasAfectadas = await connection.ExecuteAsync(sql, chequera);
            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            var sql = @"
                UPDATE GCB_CHEQUERA
                SET CHQ_Estado = 'I'
                WHERE CHQ_Chequera = :Id";

            using var connection = _connectionFactory.CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(sql, new { Id = id });
            return filasAfectadas > 0;
        }

        public async Task<bool> Reactivar(int id)
        {
            var sql = @"
                UPDATE GCB_CHEQUERA
                SET CHQ_Estado = 'A'
                WHERE CHQ_Chequera = :Id";

            using var connection = _connectionFactory.CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(sql, new { Id = id });
            return filasAfectadas > 0;
        }

        private void ValidarChequera(Chequera chequera)
        {
            if (chequera.CUB_Cuenta <= 0)
                throw new InvalidOperationException("La cuenta es obligatoria.");

            if (string.IsNullOrWhiteSpace(chequera.CHQ_Serie))
                throw new InvalidOperationException("La serie de la chequera es obligatoria.");

            if (chequera.CHQ_Numero_Desde <= 0)
                throw new InvalidOperationException("El número inicial debe ser mayor a cero.");

            if (chequera.CHQ_Numero_Hasta <= 0)
                throw new InvalidOperationException("El número final debe ser mayor a cero.");

            if (chequera.CHQ_Numero_Hasta < chequera.CHQ_Numero_Desde)
                throw new InvalidOperationException("El número hasta no puede ser menor que el número desde.");

            if (chequera.CHQ_Ultimo_Usado < 0)
                throw new InvalidOperationException("El último usado no puede ser negativo.");

            if (chequera.CHQ_Ultimo_Usado > chequera.CHQ_Numero_Hasta)
                throw new InvalidOperationException("El último usado no puede ser mayor al número hasta.");

            if (!string.IsNullOrWhiteSpace(chequera.CHQ_Estado) &&
                chequera.CHQ_Estado != "A" &&
                chequera.CHQ_Estado != "I")
            {
                throw new InvalidOperationException("El estado de la chequera solo puede ser 'A' o 'I'.");
            }
        }
    }
}
