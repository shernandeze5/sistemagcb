using Dapper;
using GestionCuentasBancarias.Domain.DTOS.ConversionMoneda;
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
    public class ConversionMonedaRepository : IConversionMonedaRepository
    {
        private readonly string connectionString;

        public ConversionMonedaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }

        private const string SELECT_BASE = @"
            SELECT
                c.COM_Conversion_Moneda,
                c.TMO_Tipo_Moneda,
                mo.TMO_Simbolo        AS TMO_Simbolo_Origen,
                mo.TMO_Descripcion    AS TMO_Descripcion_Origen,
                mo.TMO_Codigo_ISO     AS TMO_Codigo_ISO_Origen,
                c.TMO_Tipo_Moneda_Destino,
                md.TMO_Simbolo        AS TMO_Simbolo_Destino,
                md.TMO_Descripcion    AS TMO_Descripcion_Destino,
                md.TMO_Codigo_ISO     AS TMO_Codigo_ISO_Destino,
                c.COM_Tasa_Cambio,
                c.COM_Fecha_Vigencia,
                c.COM_Fuente,
                c.COM_Estado,
                c.COM_Fecha_Creacion
            FROM GCB_CONVERSION_MONEDA c
            INNER JOIN GCB_TIPO_MONEDA mo ON c.TMO_Tipo_Moneda         = mo.TMO_Tipo_Moneda
            INNER JOIN GCB_TIPO_MONEDA md ON c.TMO_Tipo_Moneda_Destino = md.TMO_Tipo_Moneda";

        public async Task<List<ResponseConversionMonedaDTO>> ObtenerConversiones()
        {
            using var connection = new OracleConnection(connectionString);
            string sql = $@"{SELECT_BASE}
                            ORDER BY c.COM_Fecha_Vigencia DESC, mo.TMO_Simbolo";
            var result = await connection.QueryAsync<ResponseConversionMonedaDTO>(sql);
            return result.ToList();
        }

        public async Task<ResponseConversionMonedaDTO> ObtenerConversionPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = $@"{SELECT_BASE}
                            WHERE c.COM_Conversion_Moneda = :Id";
            return await connection.QueryFirstOrDefaultAsync<ResponseConversionMonedaDTO>(sql, new { Id = id });
        }

        // Obtiene la tasa más reciente vigente para un par de monedas en una fecha
        public async Task<ResponseConversionMonedaDTO> ObtenerTasaVigente(
            int monedaOrigen, int monedaDestino, string fecha)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = $@"{SELECT_BASE}
                            WHERE c.TMO_Tipo_Moneda         = :Origen
                              AND c.TMO_Tipo_Moneda_Destino = :Destino
                              AND c.COM_Fecha_Vigencia      <= TO_DATE(:Fecha, 'YYYY-MM-DD')
                              AND c.COM_Estado              = 'A'
                            ORDER BY c.COM_Fecha_Vigencia DESC
                            FETCH FIRST 1 ROW ONLY";
            return await connection.QueryFirstOrDefaultAsync<ResponseConversionMonedaDTO>(sql, new
            {
                Origen = monedaOrigen,
                Destino = monedaDestino,
                Fecha = fecha
            });
        }

        public async Task CrearConversion(CreateConversionMonedaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            // Validar que no exista ya una tasa para ese par de monedas en esa fecha
            string sqlDup = @"SELECT COUNT(*) FROM GCB_CONVERSION_MONEDA
                              WHERE TMO_Tipo_Moneda         = :Origen
                                AND TMO_Tipo_Moneda_Destino = :Destino
                                AND TRUNC(COM_Fecha_Vigencia) = TRUNC(:Fecha)";

            var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
            {
                Origen = dto.TMO_Tipo_Moneda,
                Destino = dto.TMO_Tipo_Moneda_Destino,
                Fecha = dto.COM_Fecha_Vigencia
            });

            if (existe > 0)
                throw new InvalidOperationException(
                    "Ya existe una tasa de cambio para ese par de monedas en esa fecha.");

            string sql = @"INSERT INTO GCB_CONVERSION_MONEDA
                               (TMO_Tipo_Moneda, TMO_Tipo_Moneda_Destino,
                                COM_Tasa_Cambio, COM_Fecha_Vigencia, COM_Fuente)
                           VALUES
                               (:Origen, :Destino, :Tasa, :Fecha, :Fuente)";

            await connection.ExecuteAsync(sql, new
            {
                Origen = dto.TMO_Tipo_Moneda,
                Destino = dto.TMO_Tipo_Moneda_Destino,
                Tasa = dto.COM_Tasa_Cambio,
                Fecha = dto.COM_Fecha_Vigencia,
                Fuente = dto.COM_Fuente
            });
        }

        public async Task<string> ObtenerCodigoISO(int tipoMonedaId)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"SELECT TMO_Codigo_ISO 
                   FROM GCB_TIPO_MONEDA
                   WHERE TMO_Tipo_Moneda = :Id";

            return await connection.ExecuteScalarAsync<string>(sql, new { Id = tipoMonedaId });
        }
        public async Task<bool> ActualizarConversion(int id, UpdateConversionMonedaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_CONVERSION_MONEDA
                           SET COM_Tasa_Cambio    = :Tasa,
                               COM_Fecha_Vigencia = :Fecha
                           WHERE COM_Conversion_Moneda = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Tasa = dto.COM_Tasa_Cambio,
                Fecha = dto.COM_Fecha_Vigencia,
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarConversion(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_CONVERSION_MONEDA
                           SET COM_Estado = 'I'
                           WHERE COM_Conversion_Moneda = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<bool> ReactivarConversion(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_CONVERSION_MONEDA
                           SET COM_Estado = 'A'
                           WHERE COM_Conversion_Moneda = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
