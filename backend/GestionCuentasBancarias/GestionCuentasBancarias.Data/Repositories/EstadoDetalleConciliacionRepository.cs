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
    public class EstadoDetalleConciliacionRepository : IEstadoDetalleConciliacionRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public EstadoDetalleConciliacionRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<EstadoDetalleConciliacion>> ObtenerTodosAsync()
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                SELECT
                    EDC_ESTADO_DETALLE_CONCILIACION AS EDC_Estado_Detalle_Conciliacion,
                    EDC_DESCRIPCION AS EDC_Descripcion,
                    EDC_ESTADO AS EDC_Estado,
                    EDC_FECHA_CREACION AS EDC_Fecha_Creacion
                FROM GCB_ESTADO_DETALLE_CONCILIACION
                WHERE EDC_ESTADO = 1
                ORDER BY EDC_ESTADO_DETALLE_CONCILIACION";

            return await db.QueryAsync<EstadoDetalleConciliacion>(sql);
        }

        public async Task<EstadoDetalleConciliacion?> ObtenerPorIdAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                SELECT
                    EDC_ESTADO_DETALLE_CONCILIACION AS EDC_Estado_Detalle_Conciliacion,
                    EDC_DESCRIPCION AS EDC_Descripcion,
                    EDC_ESTADO AS EDC_Estado,
                    EDC_FECHA_CREACION AS EDC_Fecha_Creacion
                FROM GCB_ESTADO_DETALLE_CONCILIACION
                WHERE ESC_ESTADO_DETALLE_CONCILIACION = :Id";

            return await db.QueryFirstOrDefaultAsync<EstadoDetalleConciliacion>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(EstadoDetalleConciliacion entidad)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                INSERT INTO GCB_ESTADO_DETALLE_CONCILIACION
                (EDC_DESCRIPCION, EDC_ESTADO, EDC_FECHA_CREACION)
                VALUES
                (:EDC_Descripcion, :EDC_Estado, :EDC_Fecha_Creacion)";

            int filas = await db.ExecuteAsync(sql, entidad);

            return filas > 0;
        }

        public async Task<bool> ActualizarAsync(EstadoDetalleConciliacion entidad)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                UPDATE GCB_ESTADO_DETALLE_CONCILIACION
                SET
                    EDC_DESCRIPCION = :EDC_Descripcion,
                    EDC_ESTADO = :EDC_Estado
                WHERE EDC_ESTADO_DETALLE_CONCILIACION = :EDC_Estado_Detalle_Conciliacion";

            int filas = await db.ExecuteAsync(sql, entidad);

            return filas > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                UPDATE GCB_ESTADO_DETALLE_CONCILIACION
                SET EDC_ESTADO = 0
                WHERE EDC_ESTADO_CHEQUE = :Id";

            int filas = await db.ExecuteAsync(sql, new { Id = id });

            return filas > 0;
        }
    }
}