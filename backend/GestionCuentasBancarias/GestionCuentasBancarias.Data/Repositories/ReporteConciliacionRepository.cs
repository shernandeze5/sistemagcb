using GestionCuentasBancarias.Domain.DTOS.ReporteConciliacion;
using Microsoft.Extensions.Configuration;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class ReporteConciliacionRepository : IReporteConciliacionRepository
    {
        private readonly string connectionString;

        public ReporteConciliacionRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión OracleConnection.");
        }

        private IDbConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public async Task<IEnumerable<ReporteConciliacionDTO>> ObtenerReporte(
            int? cuentaId,
            string? periodo,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int? estadoConciliacionId)
        {
            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @"
                    SELECT
                        CO.CON_Conciliacion,
                        CO.CUB_Cuenta,
                        CB.CUB_Numero_Cuenta,
                        B.BAN_Nombre AS Banco,
                        CO.CON_Periodo,
                        CO.CON_Saldo_Banco,
                        CO.CON_Saldo_Libros,
                        CO.CON_Diferencia,
                        CO.CON_Fecha_Conciliacion,
                        CO.ECO_Estado_Conciliacion,
                        ECO.ECO_Descripcion AS EstadoConciliacion,

                        NVL(SUM(CASE 
                            WHEN UPPER(EDC.EDC_Descripcion) = 'CONCILIADO' THEN 1 
                            ELSE 0 
                        END), 0) AS TotalConciliados,

                        NVL(SUM(CASE 
                            WHEN UPPER(EDC.EDC_Descripcion) = 'PENDIENTE EN BANCO' THEN 1 
                            ELSE 0 
                        END), 0) AS TotalPendientesBanco,

                        NVL(SUM(CASE 
                            WHEN UPPER(EDC.EDC_Descripcion) = 'PENDIENTE EN LIBROS' THEN 1 
                            ELSE 0 
                        END), 0) AS TotalPendientesLibros,

                        NVL(SUM(CASE 
                            WHEN UPPER(EDC.EDC_Descripcion) = 'EN TRANSITO'
                              OR UPPER(EDC.EDC_Descripcion) = 'EN TRÁNSITO'
                            THEN 1 
                            ELSE 0 
                        END), 0) AS TotalEnTransito,

                        NVL(SUM(CASE 
                            WHEN UPPER(EDC.EDC_Descripcion) IN ('DIFERENCIA DE MONTO', 'DIFERENCIA DE FECHA') THEN 1 
                            ELSE 0 
                        END), 0) AS TotalDiferencias

                    FROM GCB_CONCILIACION CO
                    INNER JOIN GCB_CUENTA_BANCARIA CB
                        ON CB.CUB_Cuenta = CO.CUB_Cuenta
                    INNER JOIN GCB_BANCO B
                        ON B.BAN_Banco = CB.BAN_Banco
                    INNER JOIN GCB_ESTADO_CONCILIACION ECO
                        ON ECO.ECO_Estado_Conciliacion = CO.ECO_Estado_Conciliacion
                    LEFT JOIN GCB_DETALLE_CONCILIACION DC
                        ON DC.CON_Conciliacion = CO.CON_Conciliacion
                    LEFT JOIN GCB_ESTADO_DETALLE_CONCILIACION EDC
                        ON EDC.EDC_Estado_Detalle_Conciliacion = DC.EDC_Estado_Detalle_Conciliacion

                    WHERE (:CuentaId IS NULL OR CO.CUB_Cuenta = :CuentaId)
                      AND (:Periodo IS NULL OR CO.CON_Periodo = :Periodo)
                      AND (:FechaInicio IS NULL OR CO.CON_Fecha_Conciliacion >= :FechaInicio)
                      AND (:FechaFin IS NULL OR CO.CON_Fecha_Conciliacion <= :FechaFin)
                      AND (:EstadoConciliacionId IS NULL OR CO.ECO_Estado_Conciliacion = :EstadoConciliacionId)

                    GROUP BY
                        CO.CON_Conciliacion,
                        CO.CUB_Cuenta,
                        CB.CUB_Numero_Cuenta,
                        B.BAN_Nombre,
                        CO.CON_Periodo,
                        CO.CON_Saldo_Banco,
                        CO.CON_Saldo_Libros,
                        CO.CON_Diferencia,
                        CO.CON_Fecha_Conciliacion,
                        CO.ECO_Estado_Conciliacion,
                        ECO.ECO_Descripcion

                    ORDER BY CO.CON_Fecha_Conciliacion DESC";

                var resultado = await connection.QueryAsync<ReporteConciliacionDTO>(
                    sql,
                    new
                    {
                        CuentaId = cuentaId,
                        Periodo = periodo,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                        EstadoConciliacionId = estadoConciliacionId
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
