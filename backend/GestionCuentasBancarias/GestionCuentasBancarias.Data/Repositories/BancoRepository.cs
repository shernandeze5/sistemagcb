using Dapper;
using GestionCuentasBancarias.Domain.DTOS.Banco;
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
    public class BancoRepository : IBancoRepository
    {
        private readonly string connectionString;

        public BancoRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }
        public async Task<List<ResponseBancoDTO>> ObtenerBancos()
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           BAN_Banco,
                           BAN_Nombre,
                           BAN_Codigo_Swift,
                           BAN_Estado,
                           BAN_Fecha_Creacion
                           FROM GCB_BANCO
                           WHERE BAN_Estado = 'A'
                           ORDER BY BAN_Nombre";

            var result = await connection.QueryAsync<ResponseBancoDTO>(sql);
            return result.ToList();
        }

        public async Task<ResponseBancoDTO> ObtenerBancoPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT
                           BAN_Banco,
                           BAN_Nombre,
                           BAN_Codigo_Swift,
                           BAN_Estado,
                           BAN_Fecha_Creacion
                           FROM GCB_BANCO
                           WHERE BAN_Banco = :Id";

            return await connection.QueryFirstOrDefaultAsync<ResponseBancoDTO>(sql, new { Id = id });
        }

        public async Task CrearBanco(CreateBancoDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"INSERT INTO GCB_BANCO
                           (BAN_Nombre, BAN_Codigo_Swift)
                           VALUES
                           (:Nombre, :Swift)";

            await connection.ExecuteAsync(sql, new
            {
                Nombre = dto.BAN_Nombre,
                Swift = dto.BAN_Codigo_Swift
            });
        }

        public async Task<bool> ActualizarBanco(int id, UpdataBancoDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_BANCO
                           SET BAN_Nombre = :Nombre,
                               BAN_Codigo_Swift = :Swift
                           WHERE BAN_Banco = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Nombre = dto.BAN_Nombre,
                Swift = dto.BAN_Codigo_Swift,
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarBanco(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_BANCO
                           SET BAN_Estado = 'I'
                           WHERE BAN_Banco = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }
    }
}
