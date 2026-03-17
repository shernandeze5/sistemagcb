using Dapper;
using GestionCuentasBancarias.Domain.DTOS.TipoCuenta;
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
    public class TipoCuentaRespository : ITipoCuentaRepository
    {
        private readonly string connectionString;

        public TipoCuentaRespository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<IEnumerable<ResponseTipoCuentaDTO>> ObtenerTiposCuenta()
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           TCU_Tipo_Cuenta,
                           TCU_Descripcion,
                           TCU_Estado,
                           TCU_Fecha_Creacion
                           FROM GCB_TIPO_CUENTA
                           ORDER BY TCU_Descripcion";

            return await connection.QueryAsync<ResponseTipoCuentaDTO>(sql);
        }

        public async Task<ResponseTipoCuentaDTO> ObtenerTipoCuentaPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           TCU_Tipo_Cuenta,
                           TCU_Descripcion,
                           TCU_Estado,
                           TCU_Fecha_Creacion
                           FROM GCB_TIPO_CUENTA
                           WHERE TCU_Tipo_Cuenta = :Id";

            return await connection.QueryFirstOrDefaultAsync<ResponseTipoCuentaDTO>(sql, new { Id = id });
        }

        public async Task<int> CrearTipoCuenta(CreateTipoCuentaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"INSERT INTO GCB_TIPO_CUENTA
                           (TCU_Descripcion)
                           VALUES
                           (:Descripcion)";

            return await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.TCU_Descripcion
            });
        }

        public async Task<bool> ActualizarTipoCuenta(int id, UpdateTipoCuentaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_TIPO_CUENTA
                           SET TCU_Descripcion = :Descripcion
                           WHERE TCU_Tipo_Cuenta = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.TCU_Descripcion,
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarTipoCuenta(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_TIPO_CUENTA
                           SET TCU_Estado = 'I'
                           WHERE TCU_Tipo_Cuenta = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }

        public async Task<bool> ReactivarTipoCuenta(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_TIPO_CUENTA SET TCU_Estado = 'A' WHERE TCU_Tipo_Cuenta = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
