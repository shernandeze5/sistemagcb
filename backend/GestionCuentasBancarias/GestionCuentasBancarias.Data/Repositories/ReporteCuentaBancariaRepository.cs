using Dapper;
using GestionCuentasBancarias.Domain.DTOS.ReporteCuentaBancaria;
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
    public class ReporteCuentaBancariaRepository : IReporteCuentaBancariaRepository
    {
        private readonly string connectionString;

        public ReporteCuentaBancariaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión OracleConnection.");
        }

        private IDbConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public async Task<IEnumerable<ReporteCuentaBancariaDTO>> ObtenerReporte(
            int? bancoId,
            int? tipoCuentaId,
            int? tipoMonedaId,
            int? estadoCuentaId,
            string? estadoRegistro)
        {
            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @"
                    SELECT
                        C.CUB_Cuenta,
                        C.BAN_Banco,
                        B.BAN_Nombre AS Banco,
                        C.CUB_Numero_Cuenta,

                        TRIM(
                            C.CUB_Primer_Nombre || ' ' ||
                            NVL(C.CUB_Segundo_Nombre, '') || ' ' ||
                            C.CUB_Primer_Apellido || ' ' ||
                            NVL(C.CUB_Segundo_Apellido, '')
                        ) AS Titular,

                        C.TCU_Tipo_Cuenta,
                        TC.TCU_Descripcion AS TipoCuenta,

                        C.TMO_Tipo_Moneda,
                        TM.TMO_Descripcion AS TipoMoneda,

                        C.CUB_Saldo_Inicial,
                        C.CUB_Saldo_Actual,

                        C.ESC_Estado_Cuenta,
                        EC.ESC_Descripcion AS EstadoCuenta,

                        C.CUB_Estado,
                        C.CUB_Fecha_Creacion

                    FROM GCB_CUENTA_BANCARIA C
                    INNER JOIN GCB_BANCO B
                        ON B.BAN_Banco = C.BAN_Banco
                    INNER JOIN GCB_TIPO_CUENTA TC
                        ON TC.TCU_Tipo_Cuenta = C.TCU_Tipo_Cuenta
                    INNER JOIN GCB_TIPO_MONEDA TM
                        ON TM.TMO_Tipo_Moneda = C.TMO_Tipo_Moneda
                    INNER JOIN GCB_ESTADO_CUENTA EC
                        ON EC.ESC_Estado_Cuenta = C.ESC_Estado_Cuenta

                    WHERE (:BancoId IS NULL OR C.BAN_Banco = :BancoId)
                      AND (:TipoCuentaId IS NULL OR C.TCU_Tipo_Cuenta = :TipoCuentaId)
                      AND (:TipoMonedaId IS NULL OR C.TMO_Tipo_Moneda = :TipoMonedaId)
                      AND (:EstadoCuentaId IS NULL OR C.ESC_Estado_Cuenta = :EstadoCuentaId)
                      AND (:EstadoRegistro IS NULL OR C.CUB_Estado = :EstadoRegistro)

                    ORDER BY B.BAN_Nombre, C.CUB_Numero_Cuenta";

                var resultado = await connection.QueryAsync<ReporteCuentaBancariaDTO>(
                    sql,
                    new
                    {
                        BancoId = bancoId,
                        TipoCuentaId = tipoCuentaId,
                        TipoMonedaId = tipoMonedaId,
                        EstadoCuentaId = estadoCuentaId,
                        EstadoRegistro = estadoRegistro
                    },
                    transaction
                );

                transaction.Commit();
                return resultado;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
 }
