using Dapper;
using GestionCuentasBancarias.Domain.DTOS.EstadoCuenta;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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
            string sql = @"SELECT ESC_Estado_Cuenta, ESC_Descripcion, ESC_Estado, ESC_Fecha_Creacion
                           FROM GCB_ESTADO_CUENTA
                           ORDER BY ESC_Descripcion";
            return await connection.QueryAsync<ResponseEstadoCuentaDTO>(sql);
        }

        public async Task<ResponseEstadoCuentaDTO> ObtenerEstadoCuentaPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"SELECT ESC_Estado_Cuenta, ESC_Descripcion, ESC_Estado, ESC_Fecha_Creacion
                           FROM GCB_ESTADO_CUENTA
                           WHERE ESC_Estado_Cuenta = :Id";
            return await connection.QueryFirstOrDefaultAsync<ResponseEstadoCuentaDTO>(sql, new { Id = id });
        }

        public async Task<int> CrearEstadoCuenta(CreateEstadoCuentaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            var existe = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM GCB_ESTADO_CUENTA WHERE UPPER(ESC_Descripcion) = UPPER(:Descripcion)",
                new { Descripcion = dto.ESC_Descripcion });

            if (existe > 0)
                throw new InvalidOperationException("Ya existe un estado de cuenta con la misma descripción.");

            return await connection.ExecuteAsync(
                "INSERT INTO GCB_ESTADO_CUENTA (ESC_Descripcion) VALUES (:Descripcion)",
                new { Descripcion = dto.ESC_Descripcion });
        }

        public async Task<bool> ActualizarEstadoCuenta(int id, UpdateEstadoCuentaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            var existe = await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*) FROM GCB_ESTADO_CUENTA
                  WHERE UPPER(ESC_Descripcion) = UPPER(:Descripcion)
                    AND ESC_Estado_Cuenta != :Id",
                new { Descripcion = dto.ESC_Descripcion, Id = id });

            if (existe > 0)
                throw new InvalidOperationException("Ya existe otro estado de cuenta con la misma descripción.");

            var rows = await connection.ExecuteAsync(
                "UPDATE GCB_ESTADO_CUENTA SET ESC_Descripcion = :Descripcion WHERE ESC_Estado_Cuenta = :Id",
                new { Descripcion = dto.ESC_Descripcion, Id = id });

            return rows > 0;
        }

        public async Task<bool> EliminarEstadoCuenta(int id)
        {
            using var connection = new OracleConnection(connectionString);
            var rows = await connection.ExecuteAsync(
                "UPDATE GCB_ESTADO_CUENTA SET ESC_Estado = 'I' WHERE ESC_Estado_Cuenta = :Id",
                new { Id = id });
            return rows > 0;
        }

        public async Task<bool> ReactivarEstadoCuenta(int id)
        {
            using var connection = new OracleConnection(connectionString);
            var rows = await connection.ExecuteAsync(
                "UPDATE GCB_ESTADO_CUENTA SET ESC_Estado = 'A' WHERE ESC_Estado_Cuenta = :Id",
                new { Id = id });
            return rows > 0;
        }
    }
}