using Dapper;
using GestionCuentasBancarias.Domain.DTOS.DetalleConciliacion;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class EstadoDetalleConciliacionRepository : IEstadoDetalleConciliacionRepository
    {
        private readonly string connectionString;

        public EstadoDetalleConciliacionRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }
        public async Task<IEnumerable<ResponseEstadoDetalleConciliacionDTO>> ObtenerEstadosDetalleConciliacion()
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           EDC_Estado_Detalle_Conciliacion,
                           EDC_Descripcion,
                           EDC_Estado,
                           EDC_Fecha_Creacion
                           FROM GCB_ESTADO_DETALLE_CONCILIACION
                           ORDER BY EDC_Descripcion";

            return await connection.QueryAsync<ResponseEstadoDetalleConciliacionDTO>(sql);
        }

        public async Task<ResponseEstadoDetalleConciliacionDTO> ObtenerEstadoDetalleConciliacionPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           EDC_Estado_Detalle_Conciliacion,
                           EDC_Descripcion,
                           EDC_Estado,
                           EDC_Fecha_Creacion
                           FROM GCB_ESTADO_DETALLE_CONCILIACION
                           WHERE EDC_Estado_Detalle_Conciliacion = :Id";

            return await connection.QueryFirstOrDefaultAsync<ResponseEstadoDetalleConciliacionDTO>(sql, new { Id = id });
        }

        public async Task<int> CrearEstadoDetalleConciliacion(CreateEstadoDetalleConciliacionDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"INSERT INTO GCB_ESTADO_DETALLE_CONCILIACION
                           (EDC_Descripcion)
                           VALUES
                           (:Descripcion)";

            return await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.EDC_Descripcion
            });
        }

        public async Task<bool> ActualizarEstadoDetalleConciliacion(int id, UpdateEstadoDetalleConciliacionDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_ESTADO_DETALLE_CONCILIACION
                           SET EDC_Descripcion = :Descripcion
                           WHERE EDC_Estado_Detalle_Conciliacion = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.EDC_Descripcion,
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarEstadoDetalleConciliacion(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_ESTADO_DETALLE_CONCILIACION
                           SET EDC_Estado = 'I'
                           WHERE EDC_Estado_Detalle_Conciliacion = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }

        public async Task<bool> ReactivarEstadoDetalleConciliacion(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_ESTADO_DETALLE_CONCILIACION SET EDC_Estado = 'A' WHERE EDC_Estado_Detalle_Conciliacion = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}