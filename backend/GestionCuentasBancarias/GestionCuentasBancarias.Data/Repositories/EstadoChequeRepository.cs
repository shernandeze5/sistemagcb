using Dapper;
using GestionCuentasBancarias.Domain.DTOS.Cheque;
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
    public class EstadoChequeRepository : IEstadoChequeRepository
    {
        private readonly string connectionString;

        public EstadoChequeRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }
        public async Task<IEnumerable<ResponseEstadoChequeDTO>> ObtenerEstadosCheque()
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           ESC_Estado_Cheque,
                           ESC_Descripcion,
                           ESC_Estado,
                           ESC_Fecha_Creacion
                           FROM GCB_ESTADO_CHEQUE
                           ORDER BY ESC_Descripcion";

            return await connection.QueryAsync<ResponseEstadoChequeDTO>(sql);
        }

        public async Task<ResponseEstadoChequeDTO> ObtenerEstadoChequePorId(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           ESC_Estado_Cheque,
                           ESC_Descripcion,
                           ESC_Estado,
                           ESC_Fecha_Creacion
                           FROM GCB_ESTADO_CHEQUE
                           WHERE ESC_Estado_Cheque = :Id";

            return await connection.QueryFirstOrDefaultAsync<ResponseEstadoChequeDTO>(sql, new { Id = id });
        }

        public async Task<int> CrearEstadoCheque(CreateEstadoChequeDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"INSERT INTO GCB_ESTADO_CHEQUE
                           (ESC_Descripcion)
                           VALUES
                           (:Descripcion)";

            return await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.ESC_Descripcion
            });
        }

        public async Task<bool> ActualizarEstadoCheque(int id, UpdateEstadoChequeDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_ESTADO_CHEQUE
                           SET ESC_Descripcion = :Descripcion
                           WHERE ESC_Estado_Cheque = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.ESC_Descripcion,
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarEstadoCheque(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_ESTADO_CHEQUE
                           SET ESC_Estado = 'I'
                           WHERE ESC_Estado_Cheque = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }

        public async Task<bool> ReactivarEstadoCheque(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_ESTADO_CHEQUE SET ESC_Estado = 'A' WHERE ESC_Estado_Cheque = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}