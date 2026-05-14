using Dapper;
using GestionCuentasBancarias.Domain.DTOS.TipoMoneda;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class TipoMonedaRepository : ITipoMonedaRepository
    {
        private readonly string connectionString;

        public TipoMonedaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión OracleConnection.");
        }

        public async Task<IEnumerable<ResponseTipoMonedaDTO>> ObtenerTiposMoneda()
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
                SELECT
                    TMO_Tipo_Moneda,
                    TMO_Descripcion,
                    NVL(TMO_Codigo_ISO, '') AS TMO_Codigo_ISO,
                    TMO_Simbolo,
                    TMO_Estado,
                    TMO_Fecha_Creacion
                FROM GCB_TIPO_MONEDA
                ORDER BY TMO_Tipo_Moneda DESC";

            return await connection.QueryAsync<ResponseTipoMonedaDTO>(sql);
        }

        public async Task<ResponseTipoMonedaDTO?> ObtenerTipoMonedaPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
                SELECT
                    TMO_Tipo_Moneda,
                    TMO_Descripcion,
                    NVL(TMO_Codigo_ISO, '') AS TMO_Codigo_ISO,
                    TMO_Simbolo,
                    TMO_Estado,
                    TMO_Fecha_Creacion
                FROM GCB_TIPO_MONEDA
                WHERE TMO_Tipo_Moneda = :Id";

            return await connection.QueryFirstOrDefaultAsync<ResponseTipoMonedaDTO>(
                sql,
                new { Id = id }
            );
        }

        public async Task<int> CrearTipoMoneda(CreateTipoMonedaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sqlDup = @"
                SELECT COUNT(*)
                FROM GCB_TIPO_MONEDA
                WHERE UPPER(TMO_Descripcion) = UPPER(:Descripcion)
                   OR UPPER(TMO_Simbolo) = UPPER(:Simbolo)
                   OR (
                        :CodigoIso IS NOT NULL
                        AND TMO_Codigo_ISO IS NOT NULL
                        AND UPPER(TMO_Codigo_ISO) = UPPER(:CodigoIso)
                   )";

            var codigoIso = string.IsNullOrWhiteSpace(dto.TMO_Codigo_ISO)
                ? null
                : dto.TMO_Codigo_ISO.Trim().ToUpper();

            var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
            {
                Descripcion = dto.TMO_Descripcion.Trim(),
                Simbolo = dto.TMO_Simbolo.Trim(),
                CodigoIso = codigoIso
            });

            if (existe > 0)
                throw new InvalidOperationException("Ya existe una moneda con la misma descripción, símbolo o código ISO.");

            string sql = @"
                INSERT INTO GCB_TIPO_MONEDA (
                    TMO_Descripcion,
                    TMO_Codigo_ISO,
                    TMO_Simbolo,
                    TMO_Estado,
                    TMO_Fecha_Creacion
                )
                VALUES (
                    :Descripcion,
                    :CodigoIso,
                    :Simbolo,
                    'A',
                    SYSDATE
                )";

            return await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.TMO_Descripcion.Trim(),
                CodigoIso = codigoIso,
                Simbolo = dto.TMO_Simbolo.Trim()
            });
        }

        public async Task<bool> ActualizarTipoMoneda(int id, UpdateTipoMonedaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            var codigoIso = string.IsNullOrWhiteSpace(dto.TMO_Codigo_ISO)
                ? null
                : dto.TMO_Codigo_ISO.Trim().ToUpper();

            string sqlDup = @"
                SELECT COUNT(*)
                FROM GCB_TIPO_MONEDA
                WHERE (
                        UPPER(TMO_Descripcion) = UPPER(:Descripcion)
                     OR UPPER(TMO_Simbolo) = UPPER(:Simbolo)
                     OR (
                            :CodigoIso IS NOT NULL
                            AND TMO_Codigo_ISO IS NOT NULL
                            AND UPPER(TMO_Codigo_ISO) = UPPER(:CodigoIso)
                        )
                )
                AND TMO_Tipo_Moneda != :Id";

            var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
            {
                Descripcion = dto.TMO_Descripcion.Trim(),
                Simbolo = dto.TMO_Simbolo.Trim(),
                CodigoIso = codigoIso,
                Id = id
            });

            if (existe > 0)
                throw new InvalidOperationException("Ya existe otra moneda con la misma descripción, símbolo o código ISO.");

            string sql = @"
                UPDATE GCB_TIPO_MONEDA
                SET TMO_Descripcion = :Descripcion,
                    TMO_Codigo_ISO = :CodigoIso,
                    TMO_Simbolo = :Simbolo
                WHERE TMO_Tipo_Moneda = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.TMO_Descripcion.Trim(),
                CodigoIso = codigoIso,
                Simbolo = dto.TMO_Simbolo.Trim(),
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarTipoMoneda(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
                UPDATE GCB_TIPO_MONEDA
                SET TMO_Estado = 'I'
                WHERE TMO_Tipo_Moneda = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<bool> ReactivarTipoMoneda(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
                UPDATE GCB_TIPO_MONEDA
                SET TMO_Estado = 'A'
                WHERE TMO_Tipo_Moneda = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}