using Dapper;
using GestionCuentasBancarias.Domain.DTOS.ReglaRecargo;
using GestionCuentasBancarias.Domain.Entities;
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
    public class ReglaRecargoRepository : IReglaRecargoRepository
    {
        private readonly string connectionString;

        public ReglaRecargoRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }

        private const string SELECT_BASE = @"
            SELECT
                r.RCA_Regla_Recargo,
                r.CUB_Cuenta,
                c.CUB_Numero_Cuenta,
                b.BAN_Nombre,
                r.RCA_Descripcion,
                r.RCA_Origen,
                r.RCA_Monto,
                r.RCA_Frecuencia,
                r.RCA_Dia_Cobro,
                r.RCA_Estado,
                r.RCA_Fecha_Creacion
            FROM GCB_REGLA_RECARGO r
            INNER JOIN GCB_CUENTA_BANCARIA c ON r.CUB_Cuenta = c.CUB_Cuenta
            INNER JOIN GCB_BANCO b           ON c.BAN_Banco  = b.BAN_Banco";

        public async Task<List<ResponseReglaRecargoDTO>> ObtenerReglas()
        {
            using var connection = new OracleConnection(connectionString);
            string sql = $"{SELECT_BASE} ORDER BY b.BAN_Nombre, c.CUB_Numero_Cuenta";
            var result = await connection.QueryAsync<ResponseReglaRecargoDTO>(sql);
            return result.ToList();
        }

        public async Task<List<ResponseReglaRecargoDTO>> ObtenerReglasPorCuenta(int cuentaId)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = $"{SELECT_BASE} WHERE r.CUB_Cuenta = :CuentaId ORDER BY r.RCA_Origen";
            var result = await connection.QueryAsync<ResponseReglaRecargoDTO>(sql, new { CuentaId = cuentaId });
            return result.ToList();
        }

        public async Task<ResponseReglaRecargoDTO> ObtenerReglaPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = $"{SELECT_BASE} WHERE r.RCA_Regla_Recargo = :Id";
            return await connection.QueryFirstOrDefaultAsync<ResponseReglaRecargoDTO>(sql, new { Id = id });
        }

        public async Task CrearRegla(CreateReglaRecargoDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            // Validar que no exista ya una regla del mismo origen para la misma cuenta
            string sqlDup = @"SELECT COUNT(*) FROM GCB_REGLA_RECARGO
                              WHERE CUB_Cuenta  = :Cuenta
                                AND RCA_Origen  = :Origen";

            var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
            {
                Cuenta = dto.CUB_Cuenta,
                Origen = dto.RCA_Origen
            });

            if (existe > 0)
                throw new InvalidOperationException("Ya existe una regla de recargo con el mismo origen para esta cuenta.");

            string sql = @"INSERT INTO GCB_REGLA_RECARGO
                               (CUB_Cuenta, RCA_Descripcion, RCA_Origen, RCA_Monto, RCA_Frecuencia, RCA_Dia_Cobro)
                           VALUES
                               (:Cuenta, :Descripcion, :Origen, :Monto, :Frecuencia, :DiaCobro)";

            await connection.ExecuteAsync(sql, new
            {
                Cuenta = dto.CUB_Cuenta,
                Descripcion = dto.RCA_Descripcion,
                Origen = dto.RCA_Origen,
                Monto = dto.RCA_Monto,
                Frecuencia = dto.RCA_Frecuencia,
                DiaCobro = dto.RCA_Dia_Cobro
            });
        }

        public async Task<bool> ActualizarRegla(int id, UpdateReglaRecargoDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_REGLA_RECARGO
                           SET RCA_Descripcion = :Descripcion,
                               RCA_Monto       = :Monto,
                               RCA_Frecuencia  = :Frecuencia,
                               RCA_Dia_Cobro   = :DiaCobro
                           WHERE RCA_Regla_Recargo = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Descripcion = dto.RCA_Descripcion,
                Monto = dto.RCA_Monto,
                Frecuencia = dto.RCA_Frecuencia,
                DiaCobro = dto.RCA_Dia_Cobro,
                Id = id
            });

            return rows > 0;
        }

        public async Task<bool> EliminarRegla(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_REGLA_RECARGO SET RCA_Estado = 'I' WHERE RCA_Regla_Recargo = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<bool> ReactivarRegla(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_REGLA_RECARGO SET RCA_Estado = 'A' WHERE RCA_Regla_Recargo = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
