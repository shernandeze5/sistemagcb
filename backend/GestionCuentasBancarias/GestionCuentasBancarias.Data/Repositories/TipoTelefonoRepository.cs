using Dapper;
using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class TipoTelefonoRepository : ITipoTelefonoRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public TipoTelefonoRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TipoTelefono>> ObtenerTodosAsync()
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = "SELECT * FROM GCB_TIPO_TELEFONO";

            return await connection.QueryAsync<TipoTelefono>(sql);
        }

        public async Task<TipoTelefono?> ObtenerPorIdAsync(int id)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = "SELECT * FROM GCB_TIPO_TELEFONO WHERE TIT_Tipo_Telefono = :id";

            return await connection.QueryFirstOrDefaultAsync<TipoTelefono>(sql, new { id });
        }

        public async Task<bool> CrearAsync(TipoTelefono tipoTelefono)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = @"INSERT INTO GCB_TIPO_TELEFONO
                           (TIT_Descripcion, TIT_Estado, TIT_Fecha_Creacion)
                           VALUES (:TIT_Descripcion, :TIT_Estado, :TIT_Fecha_Creacion)";

            var result = await connection.ExecuteAsync(sql, tipoTelefono);

            return result > 0;
        }

        public async Task<bool> ActualizarAsync(TipoTelefono tipoTelefono)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = @"UPDATE GCB_TIPO_TELEFONO
                           SET TIT_Descripcion = :TIT_Descripcion,
                               TIT_Estado = :TIT_Estado
                           WHERE TIT_Tipo_Telefono = :TIT_Tipo_Telefono";

            var result = await connection.ExecuteAsync(sql, tipoTelefono);

            return result > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = @"UPDATE GCB_TIPO_TELEFONO
                           SET TIT_Estado = 'I'
                           WHERE TIT_Tipo_Telefono = :id";

            var result = await connection.ExecuteAsync(sql, new { id });

            return result > 0;
        }
    }
}
