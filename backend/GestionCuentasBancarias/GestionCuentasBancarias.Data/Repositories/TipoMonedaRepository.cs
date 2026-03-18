using Dapper;
using GestionCuentasBancarias.Domain.DTOS.TipoMoneda;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class TipoMonedaRepository : ITipoMonedaRepository
    {
        private readonly string connectionString;

        public TipoMonedaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<IEnumerable<ResponseTipoMonedaDTO>> ObtenerTiposMoneda()
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"SELECT
                               TMO_Tipo_Moneda,
                               TMO_Descripcion,
                               TMO_Simbolo,
                               TMO_Estado,
                               TMO_Fecha_Creacion
                           FROM GCB_TIPO_MONEDA
                           ORDER BY TMO_Descripcion";
            return await connection.QueryAsync<ResponseTipoMonedaDTO>(sql);
        }

        public async Task<ResponseTipoMonedaDTO> ObtenerTipoMonedaPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"SELECT
                               TMO_Tipo_Moneda,
                               TMO_Descripcion,
                               TMO_Simbolo,
                               TMO_Estado,
                               TMO_Fecha_Creacion
                           FROM GCB_TIPO_MONEDA
                           WHERE TMO_Tipo_Moneda = :Id";
            return await connection.QueryFirstOrDefaultAsync<ResponseTipoMonedaDTO>(sql, new { Id = id });
        }

        public async Task<int> CrearTipoMoneda(CreateTipoMonedaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"INSERT INTO GCB_TIPO_MONEDA (TMO_Descripcion, TMO_Simbolo)
                           VALUES (:Descripcion, :Simbolo)";
            return await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.TMO_Descripcion,
                Simbolo = dto.TMO_Simbolo
            });
        }

        public async Task<bool> ActualizarTipoMoneda(int id, UpdateTipoMonedaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_TIPO_MONEDA
                           SET TMO_Descripcion = :Descripcion,
                               TMO_Simbolo     = :Simbolo
                           WHERE TMO_Tipo_Moneda = :Id";
            var rows = await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.TMO_Descripcion,
                Simbolo = dto.TMO_Simbolo,
                Id = id
            });
            return rows > 0;
        }

        public async Task<bool> EliminarTipoMoneda(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_TIPO_MONEDA SET TMO_Estado = 'I' WHERE TMO_Tipo_Moneda = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<bool> ReactivarTipoMoneda(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_TIPO_MONEDA SET TMO_Estado = 'A' WHERE TMO_Tipo_Moneda = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}