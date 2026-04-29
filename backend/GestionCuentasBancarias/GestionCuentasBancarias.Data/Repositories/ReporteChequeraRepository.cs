using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Domain.DTOs.ReporteChequera;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class ReporteChequeraRepository : IReporteChequeraRepository
    {
        private readonly OracleConnectionFactory connectionFactory;

        public ReporteChequeraRepository(OracleConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<ReporteChequeraDTO>> ObtenerReporte(
            int? cuentaId,
            string? estado)
        {
            var sql = @"
                SELECT
                    CHQ_Chequera AS CHQ_Chequera,
                    CUB_Cuenta AS CUB_Cuenta,
                    CHQ_Serie AS CHQ_Serie,
                    CHQ_Numero_Desde AS CHQ_Numero_Desde,
                    CHQ_Numero_Hasta AS CHQ_Numero_Hasta,
                    CHQ_Ultimo_Usado AS CHQ_Ultimo_Usado,
                    CHQ_Estado AS CHQ_Estado,
                    CASE 
                        WHEN CHQ_Estado = 'A' THEN 'Activa'
                        WHEN CHQ_Estado = 'I' THEN 'Inactiva'
                        ELSE 'Desconocido'
                    END AS EstadoChequera,
                    CHQ_Fecha_Recepcion AS CHQ_Fecha_Recepcion,
                    CHQ_Fecha_Creacion AS CHQ_Fecha_Creacion,
                    (CHQ_Numero_Hasta - CHQ_Numero_Desde + 1) AS TotalCheques,
                    CASE 
                        WHEN CHQ_Ultimo_Usado = 0 THEN 0
                        ELSE (CHQ_Ultimo_Usado - CHQ_Numero_Desde + 1)
                    END AS ChequesUsados,
                    CASE 
                        WHEN CHQ_Ultimo_Usado = 0 THEN (CHQ_Numero_Hasta - CHQ_Numero_Desde + 1)
                        ELSE (CHQ_Numero_Hasta - CHQ_Ultimo_Usado)
                    END AS ChequesDisponibles
                FROM GCB_CHEQUERA
                WHERE (:CuentaId IS NULL OR CUB_Cuenta = :CuentaId)
                  AND (:Estado IS NULL OR CHQ_Estado = :Estado)
                ORDER BY CHQ_Chequera DESC";

            using var connection = connectionFactory.CreateConnection();

            return await connection.QueryAsync<ReporteChequeraDTO>(
                sql,
                new
                {
                    CuentaId = cuentaId,
                    Estado = estado
                });
        }
    }
}