using Dapper;
using GestionCuentasBancarias.Domain.DTOS.InteresFrecuencia;
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
    public class InteresFrecuenciaRepository : IinteresFrecuenciaRepository
    {
        private readonly string connectionString;

        public InteresFrecuenciaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<List<ResponseInteresFrecuenciaDTO>> ObtenerFrecuencias()
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"SELECT
                               INF_Interes_Frecuencia,
                               INF_Descripcion,
                               INF_Estado,
                               INF_Fecha_Creacion
                           FROM GCB_INTERES_FRECUENCIA
                           ORDER BY INF_Descripcion";
            var result = await connection.QueryAsync<ResponseInteresFrecuenciaDTO>(sql);
            return result.ToList();
        }

        public async Task<ResponseInteresFrecuenciaDTO> ObtenerFrecuenciaPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"SELECT
                               INF_Interes_Frecuencia,
                               INF_Descripcion,
                               INF_Estado,
                               INF_Fecha_Creacion
                           FROM GCB_INTERES_FRECUENCIA
                           WHERE INF_Interes_Frecuencia = :Id";
            return await connection.QueryFirstOrDefaultAsync<ResponseInteresFrecuenciaDTO>(sql, new { Id = id });
        }

        public async Task CrearFrecuencia(CreateInteresFrecuenciaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sqlDup = @"SELECT COUNT(*) FROM GCB_INTERES_FRECUENCIA
                              WHERE UPPER(INF_Descripcion) = UPPER(:Descripcion)";

            var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
            {
                Descripcion = dto.INF_Descripcion
            });

            if (existe > 0)
                throw new InvalidOperationException("Ya existe una frecuencia con la misma descripción.");

            string sql = @"INSERT INTO GCB_INTERES_FRECUENCIA (INF_Descripcion)
                           VALUES (:Descripcion)";

            await connection.ExecuteAsync(sql, new { Descripcion = dto.INF_Descripcion });
        }

        public async Task<bool> ActualizarFrecuencia(int id, UpdateInteresFrecuenciaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sqlDup = @"SELECT COUNT(*) FROM GCB_INTERES_FRECUENCIA
                              WHERE UPPER(INF_Descripcion) = UPPER(:Descripcion)
                                AND INF_Interes_Frecuencia != :Id";

            var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
            {
                Descripcion = dto.INF_Descripcion,
                Id = id
            });

            if (existe > 0)
                throw new InvalidOperationException("Ya existe otra frecuencia con la misma descripción.");

            string sql = @"UPDATE GCB_INTERES_FRECUENCIA
                           SET INF_Descripcion = :Descripcion
                           WHERE INF_Interes_Frecuencia = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.INF_Descripcion,
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarFrecuencia(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_INTERES_FRECUENCIA SET INF_Estado = 'I' WHERE INF_Interes_Frecuencia = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<bool> ReactivarFrecuencia(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_INTERES_FRECUENCIA SET INF_Estado = 'A' WHERE INF_Interes_Frecuencia = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
