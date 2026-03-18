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
    public class EstadoConciliacionRepository : IEstadoConciliacionRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public EstadoConciliacionRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<EstadoConciliacion>> ObtenerTodosAsync()
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                SELECT
                    ECO_ESTADO_CONCILIACION AS ECO_Estado_Conciliacion,
                    ECO_DESCRIPCION AS ECO_Descripcion,
                    ECO_ESTADO AS ECO_Estado,
                    ECO_FECHA_CREACION AS ECO_Fecha_Creacion
                FROM GCB_ESTADO_CONCILIACION
                WHERE ECO_ESTADO = 1
                ORDER BY ECO_ESTADO_CONCILIACION";

            return await db.QueryAsync<EstadoConciliacion>(sql);
        }

        public async Task<EstadoConciliacion?> ObtenerPorIdAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                SELECT
                    ECO_ESTADO_CONCILIACION AS ECO_Estado_Conciliacion,
                    ECO_DESCRIPCION AS ECO_Descripcion,
                    ECO_ESTADO AS ECO_Estado,
                    ECP_FECHA_CREACION AS ECP_Fecha_Creacion
                FROM GCB_ESTADO_CONCILIACION
                WHERE ESC_ESTADO_CONCILIACION = :Id";

            return await db.QueryFirstOrDefaultAsync<EstadoConciliacion>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(EstadoConciliacion entidad)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                INSERT INTO GCB_ESTADO_CONCILIACION
                (ECO_DESCRIPCION, ECO_ESTADO, ECO_FECHA_CREACION)
                VALUES
                (:ECO_Descripcion, :ECO_Estado, :ECO_Fecha_Creacion)";

            int filas = await db.ExecuteAsync(sql, entidad);

            return filas > 0;
        }

        public async Task<bool> ActualizarAsync(EstadoConciliacion entidad)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                UPDATE GCB_ESTADO_CONCILIACION
                SET
                    ECO_DESCRIPCION = :ECO_Descripcion,
                    ECO_ESTADO = :ECO_Estado
                WHERE ECO_ESTADO_CONCILIACION = :ESC_Estado_Conciliacion";

            int filas = await db.ExecuteAsync(sql, entidad);

            return filas > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                UPDATE GCB_ESTADO_CONCILIACION
                SET ECO_ESTADO = 0
                WHERE ECO_ESTADO_CONCILIACION = :Id";

            int filas = await db.ExecuteAsync(sql, new { Id = id });

            return filas > 0;
        }
    }
}