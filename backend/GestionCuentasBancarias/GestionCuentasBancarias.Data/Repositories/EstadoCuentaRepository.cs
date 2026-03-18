using Dapper;
using GestionCuentasBancarias.Domain.DTOS.EstadoCuenta;
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
    public class EstadoCuentaRepository : IEstadoCuentaRepository
    {
        private readonly string connectionString;

        public EstadoCuentaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }
        public async Task<IEnumerable<ResponseEstadoCuentaDTO>> ObtenerEstadosCuenta()
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           ESC_Estado_Cuenta,
                           ESC_Descripcion,
                           ESC_Estado,
                           ESC_Fecha_Creacion
                           FROM GCB_ESTADO_CUENTA
                           ORDER BY ESC_Descripcion";

            return await connection.QueryAsync<ResponseEstadoCuentaDTO>(sql);
        }

        public async Task<ResponseEstadoCuentaDTO> ObtenerEstadoCuentaPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           ESC_Estado_Cuenta,
                           ESC_Descripcion,
                           ESC_Estado,
                           ESC_Fecha_Creacion
                           FROM GCB_ESTADO_CUENTA
                           WHERE ESC_Estado_Cuenta = :Id";

            return await connection.QueryFirstOrDefaultAsync<ResponseEstadoCuentaDTO>(sql, new { Id = id });
        }

        public async Task<int> CrearEstadoCuenta(CreateEstadoCuentaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"INSERT INTO GCB_ESTADO_CUENTA
                           (ESC_Descripcion)
                           VALUES
                           (:Descripcion)";

            return await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.ESC_Descripcion
            });
        }

        public async Task<bool> ActualizarEstadoCuenta(int id, UpdateEstadoCuentaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_ESTADO_CUENTA
                           SET ESC_Descripcion = :Descripcion
                           WHERE ESC_Estado_Cuenta = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.ESC_Descripcion,
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarEstadoCuenta(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_ESTADO_CUENTA
                           SET ESC_Estado = 'I'
                           WHERE ESC_Estado_Cuenta = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }

        public async Task<bool> ReactivarEstadoCuenta(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_ESTADO_CUENTA SET ESC_Estado = 'A' WHERE ESC_Estado_Cuenta = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
