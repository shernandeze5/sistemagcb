using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class TipoDireccionRepository : ITipoDireccionRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public TipoDireccionRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TipoDireccion>> ObtenerTodosAsync()
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = "SELECT * FROM GCB_TIPO_DIRECCION";

            return await connection.QueryAsync<TipoDireccion>(sql);
        }

        public async Task<TipoDireccion?> ObtenerPorIdAsync(int id)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = "SELECT * FROM GCB_TIPO_DIRECCION WHERE TDI_Tipo_Direccion = :id";

            return await connection.QueryFirstOrDefaultAsync<TipoDireccion>(sql, new { id });
        }

        public async Task<bool> CrearAsync(TipoDireccion tipoDireccion)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = @"INSERT INTO GCB_TIPO_DIRECCION
                           (TDI_Descripcion, TDI_Estado, TDI_Fecha_Creacion)
                           VALUES (:TDI_Descripcion, :TDI_Estado, :TDI_Fecha_Creacion)";

            var result = await connection.ExecuteAsync(sql, tipoDireccion);

            return result > 0;
        }

        public async Task<bool> ActualizarAsync(TipoDireccion tipoDireccion)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = @"UPDATE GCB_TIPO_DIRECCION
                           SET TDI_Descripcion = :TDI_Descripcion,
                               TDI_Estado = :TDI_Estado
                           WHERE TDI_Tipo_Direccion = :TDI_Tipo_Direccion";

            var result = await connection.ExecuteAsync(sql, tipoDireccion);

            return result > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = @"UPDATE GCB_TIPO_DIRECCION
                           SET TDI_Estado = 'I'
                           WHERE TDI_Tipo_Direccion = :id";

            var result = await connection.ExecuteAsync(sql, new { id });

            return result > 0;
        }
    }
}
