using Dapper;
using GestionCuentasBancarias.Domain.DTOS.TasaInteres;
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
    public class TasaInteresRepository : ITasaInteresRepository
    {
            private readonly string connectionString;

            public TasaInteresRepository(IConfiguration configuration)
            {
                connectionString = configuration.GetConnectionString("OracleConnection");
            }

            public async Task<List<ResponseTasaInteresDTO>> ObtenerTasas()
            {
                using var connection = new OracleConnection(connectionString);
                string sql = @"SELECT
                               t.TIN_Tasa_Interes,
                               t.CUB_Cuenta,
                               c.CUB_Numero_Cuenta,
                               b.BAN_Nombre,
                               tc.TCU_Descripcion,
                               t.INF_Frecuencia,
                               f.INF_Descripcion,
                               t.TIN_Porcentaje,
                               t.TIN_Estado,
                               t.TIN_Fecha_Creacion
                           FROM GCB_TASA_INTERES t
                           INNER JOIN GCB_CUENTA_BANCARIA c ON t.CUB_Cuenta = c.CUB_Cuenta
                           INNER JOIN GCB_BANCO b           ON c.BAN_Banco  = b.BAN_Banco
                           INNER JOIN GCB_TIPO_CUENTA tc    ON c.TCU_Tipo_Cuenta = tc.TCU_Tipo_Cuenta
                           INNER JOIN GCB_INTERES_FRECUENCIA f ON t.INF_Frecuencia = f.INF_Interes_Frecuencia
                           ORDER BY b.BAN_Nombre, c.CUB_Numero_Cuenta";
                var result = await connection.QueryAsync<ResponseTasaInteresDTO>(sql);
                return result.ToList();
            }

            public async Task<ResponseTasaInteresDTO> ObtenerTasaPorId(int id)
            {
                using var connection = new OracleConnection(connectionString);
                string sql = @"SELECT
                               t.TIN_Tasa_Interes,
                               t.CUB_Cuenta,
                               c.CUB_Numero_Cuenta,
                               b.BAN_Nombre,
                               tc.TCU_Descripcion,
                               t.INF_Frecuencia,
                               f.INF_Descripcion,
                               t.TIN_Porcentaje,
                               t.TIN_Estado,
                               t.TIN_Fecha_Creacion
                           FROM GCB_TASA_INTERES t
                           INNER JOIN GCB_CUENTA_BANCARIA c ON t.CUB_Cuenta = c.CUB_Cuenta
                           INNER JOIN GCB_BANCO b           ON c.BAN_Banco  = b.BAN_Banco
                           INNER JOIN GCB_TIPO_CUENTA tc    ON c.TCU_Tipo_Cuenta = tc.TCU_Tipo_Cuenta
                           INNER JOIN GCB_INTERES_FRECUENCIA f ON t.INF_Frecuencia = f.INF_Interes_Frecuencia
                           WHERE t.TIN_Tasa_Interes = :Id";
                return await connection.QueryFirstOrDefaultAsync<ResponseTasaInteresDTO>(sql, new { Id = id });
            }

            public async Task CrearTasa(CreateTasaInteresDTO dto)
            {
                using var connection = new OracleConnection(connectionString);

                // Validar que no exista ya una tasa para esa cuenta
                string sqlDup = @"SELECT COUNT(*) FROM GCB_TASA_INTERES
                              WHERE CUB_Cuenta = :Cuenta";

                var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
                {
                    Cuenta = dto.CUB_Cuenta
                });

                if (existe > 0)
                    throw new InvalidOperationException("Ya existe una tasa de interés para esta cuenta.");

                string sql = @"INSERT INTO GCB_TASA_INTERES
                               (CUB_Cuenta, INF_Frecuencia, TIN_Porcentaje)
                           VALUES
                               (:Cuenta, :Frecuencia, :Porcentaje)";

                await connection.ExecuteAsync(sql, new
                {
                    Cuenta = dto.CUB_Cuenta,
                    Frecuencia = dto.INF_Frecuencia,
                    Porcentaje = dto.TIN_Porcentaje
                });
            }

            public async Task<bool> ActualizarTasa(int id, UpdateTasaInteresDTO dto)
            {
                using var connection = new OracleConnection(connectionString);

                string sql = @"UPDATE GCB_TASA_INTERES
                           SET INF_Frecuencia  = :Frecuencia,
                               TIN_Porcentaje  = :Porcentaje
                           WHERE TIN_Tasa_Interes = :Id";

                var rows = await connection.ExecuteAsync(sql, new
                {
                    Frecuencia = dto.INF_Frecuencia,
                    Porcentaje = dto.TIN_Porcentaje,
                    Id = id
                });

                return rows > 0;
            }

            public async Task<bool> EliminarTasa(int id)
            {
                using var connection = new OracleConnection(connectionString);
                string sql = @"UPDATE GCB_TASA_INTERES SET TIN_Estado = 'I' WHERE TIN_Tasa_Interes = :Id";
                var rows = await connection.ExecuteAsync(sql, new { Id = id });
                return rows > 0;
            }

            public async Task<bool> ReactivarTasa(int id)
            {
                using var connection = new OracleConnection(connectionString);
                string sql = @"UPDATE GCB_TASA_INTERES SET TIN_Estado = 'A' WHERE TIN_Tasa_Interes = :Id";
                var rows = await connection.ExecuteAsync(sql, new { Id = id });
                return rows > 0;
            }
        
    }
}
