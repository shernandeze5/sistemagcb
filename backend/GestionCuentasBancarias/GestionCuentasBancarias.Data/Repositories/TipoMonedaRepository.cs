using Dapper;
using GestionCuentasBancarias.Domain.DTOS.TipoMoneda;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
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
                               TMO_Codigo_ISO,
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
                               TMO_Codigo_ISO,
                               TMO_Estado,
                               TMO_Fecha_Creacion
                           FROM GCB_TIPO_MONEDA
                           WHERE TMO_Tipo_Moneda = :Id";
            return await connection.QueryFirstOrDefaultAsync<ResponseTipoMonedaDTO>(sql, new { Id = id });
        }

        public async Task<int> CrearTipoMoneda(CreateTipoMonedaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            // Validar duplicado: misma descripción O mismo símbolo
            string sqlDup = @"SELECT COUNT(*) FROM GCB_TIPO_MONEDA
                              WHERE UPPER(TMO_Descripcion) = UPPER(:Descripcion)
                                 OR UPPER(TMO_Simbolo)     = UPPER(:Simbolo)";

            var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
            {
                Descripcion = dto.TMO_Descripcion,
                Simbolo = dto.TMO_Simbolo
            });

            if (existe > 0)
                throw new InvalidOperationException("Ya existe un tipo de moneda con la misma descripción o símbolo.");

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

            // Validar duplicado excluyendo el propio registro
            string sqlDup = @"SELECT COUNT(*) FROM GCB_TIPO_MONEDA
                              WHERE (UPPER(TMO_Descripcion) = UPPER(:Descripcion)
                                  OR UPPER(TMO_Simbolo)     = UPPER(:Simbolo))
                                AND TMO_Tipo_Moneda != :Id";

            var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
            {
                Descripcion = dto.TMO_Descripcion,
                Simbolo = dto.TMO_Simbolo,
                Id = id
            });

            if (existe > 0)
                throw new InvalidOperationException("Ya existe otro tipo de moneda con la misma descripción o símbolo.");

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