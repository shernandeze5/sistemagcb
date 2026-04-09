using Dapper;
using GestionCuentasBancarias.Domain.DTOS.ReglaRecargo;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<int> Crear(CreateReglaRecargoDTO dto)
        {
            using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();

            string sql = @"
                INSERT INTO GCB_REGLA_RECARGO
                (CUB_Cuenta, RCA_Descripcion, RCA_Origen, RCA_Monto, RCA_Frecuencia, RCA_Dia_Cobro)
                VALUES (:Cuenta, :Descripcion, :Origen, :Monto, :Frecuencia, :Dia)
                RETURNING RCA_Regla_Recargo INTO :Id";

            var parameters = new DynamicParameters();

            parameters.Add("Cuenta", dto.CUB_Cuenta);
            parameters.Add("Descripcion", dto.RCA_Descripcion);
            parameters.Add("Origen", dto.RCA_Origen);
            parameters.Add("Monto", dto.RCA_Monto);
            parameters.Add("Frecuencia", dto.RCA_Frecuencia);
            parameters.Add("Dia", dto.RCA_Dia_Cobro);

            parameters.Add("Id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            await connection.ExecuteAsync(sql, parameters);

            return parameters.Get<int>("Id");
        }

        public async Task<IEnumerable<ResponseReglaRecargoDTO>> ObtenerPorCuenta(int cuentaId)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
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
        INNER JOIN GCB_BANCO b ON c.BAN_Banco = b.BAN_Banco
        WHERE r.CUB_Cuenta = :Cuenta";

            return await connection.QueryAsync<ResponseReglaRecargoDTO>(sql, new { Cuenta = cuentaId });
        }

        public async Task Actualizar(int id, UpdateReglaRecargoDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"
        UPDATE GCB_REGLA_RECARGO SET
            RCA_Descripcion = :Descripcion,
            RCA_Origen = :Origen,
            RCA_Monto = :Monto,
            RCA_Frecuencia = :Frecuencia,
            RCA_Dia_Cobro = :Dia
        WHERE RCA_Regla_Recargo = :Id";

            await connection.ExecuteAsync(sql, new
            {
                Id = id,
                Descripcion = dto.RCA_Descripcion,
                Origen = dto.RCA_Origen,
                Monto = dto.RCA_Monto,
                Frecuencia = dto.RCA_Frecuencia,
                Dia = dto.RCA_Dia_Cobro
            });
        }

        public async Task Eliminar(int id)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_REGLA_RECARGO SET RCA_Estado = 'I' WHERE RCA_Regla_Recargo = :Id";

            await connection.ExecuteAsync(sql, new { Id = id });
        }

        // 🔥 AUTOMÁTICO
        public async Task AplicarRecargoAutomatico(int cuentaId)
        {
            using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();
            using var trx = connection.BeginTransaction();

            try
            {
                var regla = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    @"SELECT * FROM GCB_REGLA_RECARGO 
                  WHERE CUB_Cuenta = :Cuenta AND RCA_Estado = 'A'",
                    new { Cuenta = cuentaId }, trx);

                if (regla == null) return;

                if (!DebeAplicarRecargo(regla)) return;

                decimal saldo = await connection.ExecuteScalarAsync<decimal>(
                    @"SELECT CUB_Saldo_Actual FROM GCB_CUENTA_BANCARIA WHERE CUB_Cuenta = :Id",
                    new { Id = cuentaId }, trx);

                decimal nuevoSaldo = saldo - (decimal)regla.RCA_Monto;

                await connection.ExecuteAsync(@"
                INSERT INTO GCB_MOVIMIENTO
                (CUB_Cuenta, TIM_Tipo_Movimiento, MEM_Medio_MOVIMIENTO, MOV_Fecha,
                 MOV_Descripcion, MOV_Monto, MOV_Saldo, ESM_Estado_Movimiento)
                VALUES (:Cuenta, 2, 1, SYSDATE,
                        'Recargo automático', :Monto, :Saldo, 1)",
                    new
                    {
                        Cuenta = cuentaId,
                        Monto = -(decimal)regla.RCA_Monto,
                        Saldo = nuevoSaldo
                    }, trx);

                await connection.ExecuteAsync(@"
                UPDATE GCB_CUENTA_BANCARIA 
                SET CUB_Saldo_Actual = :Saldo 
                WHERE CUB_Cuenta = :Cuenta",
                    new { Saldo = nuevoSaldo, Cuenta = cuentaId }, trx);

                trx.Commit();
            }
            catch
            {
                trx.Rollback();
                throw;
            }
        }

        private bool DebeAplicarRecargo(dynamic regla)
        {
            var hoy = DateTime.Now;

            return regla.RCA_Frecuencia switch
            {
                "M" => regla.RCA_Dia_Cobro == hoy.Day,
                "U" => true,
                "O" => true,
                _ => false
            };
        }
    }
}
