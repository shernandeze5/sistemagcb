using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Domain.DTOs.ReporteCheque;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class ReporteChequeRepository : IReporteChequeRepository
    {
        private readonly OracleConnectionFactory connectionFactory;

        public ReporteChequeRepository(OracleConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<ReporteChequeDTO>> ObtenerReporte(
            int? cuentaId,
            int? estadoChequeId,
            int? chequeraId,
            int? personaId)
        {
            var sql = @"
                SELECT
                    C.CHE_Cheque AS CHE_Cheque,
                    C.MOV_Movimiento AS MOV_Movimiento,
                    M.CUB_Cuenta AS CUB_Cuenta,
                    C.CHQ_Chequera AS CHQ_Chequera,
                    M.PER_Persona AS PER_Persona,

                    CASE
                        WHEN P.PER_Razon_Social IS NOT NULL AND TRIM(P.PER_Razon_Social) <> '' THEN P.PER_Razon_Social
                        ELSE TRIM(
                            NVL(P.PER_Primer_Nombre, '') || ' ' ||
                            NVL(P.PER_Segundo_Nombre, '') || ' ' ||
                            NVL(P.PER_Primer_Apellido, '') || ' ' ||
                            NVL(P.PER_Segundo_Apellido, '')
                        )
                    END AS Beneficiario,

                    C.CHE_Numero_Cheque AS CHE_Numero_Cheque,
                    C.CHE_Monto_Letras AS CHE_Monto_Letras,
                    C.CHE_Fecha_Emision AS CHE_Fecha_Emision,
                    C.CHE_Fecha_Cobro AS CHE_Fecha_Cobro,
                    C.CHE_Fecha_Vencimiento AS CHE_Fecha_Vencimiento,
                    C.CHE_Concepto AS CHE_Concepto,

                    C.ESC_Estado_Cheque AS ESC_Estado_Cheque,
                    EC.ESC_Descripcion AS EstadoCheque,

                    M.MOV_Fecha AS MOV_Fecha,
                    NVL(M.MOV_Numero_Referencia, '') AS MOV_Numero_Referencia,
                    NVL(M.MOV_Descripcion, '') AS MOV_Descripcion,
                    M.MOV_Monto AS MOV_Monto,
                    M.MOV_Monto_Origen AS MOV_Monto_Origen,
                    M.MOV_Saldo AS MOV_Saldo

                FROM GCB_CHEQUE C
                INNER JOIN GCB_MOVIMIENTO M
                    ON M.MOV_Movimiento = C.MOV_Movimiento
                LEFT JOIN GCB_PERSONA P
                    ON P.PER_Persona = M.PER_Persona
                INNER JOIN GCB_ESTADO_CHEQUE EC
                    ON EC.ESC_Estado_Cheque = C.ESC_Estado_Cheque

                WHERE (:CuentaId IS NULL OR M.CUB_Cuenta = :CuentaId)
                  AND (:EstadoChequeId IS NULL OR C.ESC_Estado_Cheque = :EstadoChequeId)
                  AND (:ChequeraId IS NULL OR C.CHQ_Chequera = :ChequeraId)
                  AND (:PersonaId IS NULL OR M.PER_Persona = :PersonaId)

                ORDER BY C.CHE_Cheque DESC";

            using var connection = connectionFactory.CreateConnection();

            return await connection.QueryAsync<ReporteChequeDTO>(
                sql,
                new
                {
                    CuentaId = cuentaId,
                    EstadoChequeId = estadoChequeId,
                    ChequeraId = chequeraId,
                    PersonaId = personaId
                });
        }
    }
}