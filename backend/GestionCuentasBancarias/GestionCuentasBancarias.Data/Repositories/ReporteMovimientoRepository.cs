using GestionCuentasBancarias.Domain.DTOS.ReporteMovimiento;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class ReporteMovimientoRepository : IReporteMovimientoRepository
    {
        private readonly string connectionString;

        public ReporteMovimientoRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión OracleConnection.");
        }

        private IDbConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public async Task<IEnumerable<ReporteMovimientoDTO>> ObtenerReporte(
            int? cuentaId,
            int? tipoMovimientoId,
            int? medioMovimientoId,
            int? estadoMovimientoId,
            int? personaId)
        {
            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @"
                    SELECT
                        M.MOV_Movimiento,
                        M.CUB_Cuenta,
                        C.CUB_Numero_Cuenta,

                        M.PER_Persona,

                        CASE
                            WHEN P.PER_Razon_Social IS NOT NULL THEN P.PER_Razon_Social
                            ELSE TRIM(
                                NVL(P.PER_Primer_Nombre, '') || ' ' ||
                                NVL(P.PER_Segundo_Nombre, '') || ' ' ||
                                NVL(P.PER_Primer_Apellido, '') || ' ' ||
                                NVL(P.PER_Segundo_Apellido, '')
                            )
                        END AS Persona,

                        M.TIM_Tipo_Movimiento,
                        TM.TIM_Descripcion AS TipoMovimiento,

                        M.MEM_Medio_Movimiento,
                        MM.MEM_Descripcion AS MedioMovimiento,

                        M.ESM_Estado_Movimiento,
                        EM.ESM_Descripcion AS EstadoMovimiento,

                        M.MOV_Monto_Origen,
                        0 AS MOV_Recargo,
                        M.MOV_Monto,
                        M.MOV_Saldo,
                        M.MOV_Fecha,
                        M.MOV_Numero_Referencia,
                        M.MOV_Descripcion,
                        M.MOV_Fecha_Creacion

                    FROM GCB_MOVIMIENTO M
                    INNER JOIN GCB_CUENTA_BANCARIA C
                        ON C.CUB_Cuenta = M.CUB_Cuenta
                    LEFT JOIN GCB_PERSONA P
                        ON P.PER_Persona = M.PER_Persona
                    INNER JOIN GCB_TIPO_MOVIMIENTO TM
                        ON TM.TIM_Tipo_Movimiento = M.TIM_Tipo_Movimiento
                    INNER JOIN GCB_MEDIO_MOVIMIENTO MM
                        ON MM.MEM_Medio_Movimiento = M.MEM_Medio_Movimiento
                    INNER JOIN GCB_ESTADO_MOVIMIENTO EM
                        ON EM.ESM_Estado_Movimiento = M.ESM_Estado_Movimiento

                    WHERE (:CuentaId IS NULL OR M.CUB_Cuenta = :CuentaId)
                      AND (:TipoMovimientoId IS NULL OR M.TIM_Tipo_Movimiento = :TipoMovimientoId)
                      AND (:MedioMovimientoId IS NULL OR M.MEM_Medio_Movimiento = :MedioMovimientoId)
                      AND (:EstadoMovimientoId IS NULL OR M.ESM_Estado_Movimiento = :EstadoMovimientoId)
                      AND (:PersonaId IS NULL OR M.PER_Persona = :PersonaId)

                    ORDER BY M.MOV_Fecha DESC, M.MOV_Movimiento DESC";

                var resultado = await connection.QueryAsync<ReporteMovimientoDTO>(
                    sql,
                    new
                    {
                        CuentaId = cuentaId,
                        TipoMovimientoId = tipoMovimientoId,
                        MedioMovimientoId = medioMovimientoId,
                        EstadoMovimientoId = estadoMovimientoId,
                        PersonaId = personaId
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
