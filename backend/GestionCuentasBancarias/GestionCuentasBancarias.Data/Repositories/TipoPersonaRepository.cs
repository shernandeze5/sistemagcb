using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class TipoPersonaRepository : ITipoPersonaRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public TipoPersonaRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TipoPersona>> ObtenerTodosAsync()
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                SELECT
                    TIP_TIPO_PERSONA AS TIP_Tipo_Persona,
                    TIP_DESCRIPCION AS TIP_Descripcion,
                    TIP_ESTADO AS TIP_Estado,
                    TIP_FECHA_CREACION AS TIP_Fecha_Creacion
                FROM GCB_TIPO_PERSONA
                ORDER BY TIP_TIPO_PERSONA";

            return await db.QueryAsync<TipoPersona>(sql);
        }

        public async Task<TipoPersona?> ObtenerPorIdAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                SELECT
                    TIP_TIPO_PERSONA AS TIP_Tipo_Persona,
                    TIP_DESCRIPCION AS TIP_Descripcion,
                    TIP_ESTADO AS TIP_Estado,
                    TIP_FECHA_CREACION AS TIP_Fecha_Creacion
                FROM GCB_TIPO_PERSONA
                WHERE TIP_TIPO_PERSONA = :Id";

            return await db.QueryFirstOrDefaultAsync<TipoPersona>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(TipoPersona entidad)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                INSERT INTO GCB_TIPO_PERSONA
                (TIP_DESCRIPCION, TIP_ESTADO, TIP_FECHA_CREACION)
                VALUES
                (:TIP_Descripcion, :TIP_Estado, :TIP_Fecha_Creacion)";

            int filas = await db.ExecuteAsync(sql, entidad);

            return filas > 0;
        }

        public async Task<bool> ActualizarAsync(TipoPersona entidad)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                UPDATE GCB_TIPO_PERSONA
                SET
                    TIP_DESCRIPCION = :TIP_Descripcion,
                    TIP_ESTADO = :TIP_Estado
                WHERE TIP_TIPO_PERSONA = :TIP_Tipo_Persona";

            int filas = await db.ExecuteAsync(sql, entidad);

            return filas > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                UPDATE GCB_TIPO_PERSONA
                SET TIP_ESTADO = 'I'
                WHERE TIP_TIPO_PERSONA = :Id";

            int filas = await db.ExecuteAsync(sql, new { Id = id });

            return filas > 0;
        }
    }
}
