using Dapper;
using GestionCuentasBancarias.Domain.DTOS.Conciliacion;
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
    public class EstadoConciliacionRepository : IEstadoConciliacionRepository
    {
        private readonly string connectionString;

        public EstadoConciliacionRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }
        public async Task<IEnumerable<ResponseEstadoConciliacionDTO>> ObtenerEstadosConciliacion()
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           ECO_Estado_Conciliacion,
                           ECO_Descripcion,
                           ECO_Estado,
                           ECO_Fecha_Creacion
                           FROM GCB_ESTADO_CONCILIACION
                           ORDER BY ECO_Descripcion";

            return await connection.QueryAsync<ResponseEstadoConciliacionDTO>(sql);
        }

        public async Task<ResponseEstadoConciliacionDTO> ObtenerEstadoConciliacionPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           ECO_Estado_Conciliacion,
                           ECO_Descripcion,
                           ECO_Estado,
                           ECO_Fecha_Creacion
                           FROM GCB_ESTADO_CONCILIACION
                           WHERE ECO_Estado_Conciliacion = :Id";

            return await connection.QueryFirstOrDefaultAsync<ResponseEstadoConciliacionDTO>(sql, new { Id = id });
        }

        public async Task<int> CrearEstadoConciliacion(CreateEstadoConciliacionDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"INSERT INTO GCB_ESTADO_CONCILIACION
                           (ECO_Descripcion)
                           VALUES
                           (:Descripcion)";

            return await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.ECO_Descripcion
            });
        }

        public async Task<bool> ActualizarEstadoConciliacion(int id, UpdateEstadoConciliacionDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_ESTADO_CONCILIACION
                           SET ECO_Descripcion = :Descripcion
                           WHERE ECO_Estado_Conciliacion = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.ECO_Descripcion,
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarEstadoConciliacion(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_ESTADO_CONCILIACION
                           SET ECO_Estado = 'I'
                           WHERE ECO_Estado_Conciliacion = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }

        public async Task<bool> ReactivarEstadoConciliacion(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_ESTADO_CONCILIACION SET ECO_Estado = 'A' WHERE ECO_Estado_Conciliacion = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}