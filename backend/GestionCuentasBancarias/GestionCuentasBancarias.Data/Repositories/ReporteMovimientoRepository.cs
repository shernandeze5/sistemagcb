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

        public async Task<EstadoCuentaMovimientoDTO> ObtenerReporte(
             int? cuentaId,
             int? tipoMovimientoId,
             int? medioMovimientoId,
             int? estadoMovimientoId,
             int? personaId,
             DateTime? fechaInicio,
             DateTime? fechaFin)
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

                        B.BAN_Nombre,

                        C.CUB_Primer_Nombre,
                        C.CUB_Segundo_Nombre,
                        C.CUB_Primer_Apellido,
                        C.CUB_Segundo_Apellido,

                        TC.TCU_Descripcion,
                        TMO.TMO_Descripcion,
                        TMO.TMO_Simbolo,

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

                    INNER JOIN GCB_BANCO B
                        ON B.BAN_Banco = C.BAN_Banco

                    INNER JOIN GCB_TIPO_CUENTA TC
                        ON TC.TCU_Tipo_Cuenta = C.TCU_Tipo_Cuenta

                    INNER JOIN GCB_TIPO_MONEDA TMO
                        ON TMO.TMO_Tipo_Moneda = C.TMO_Tipo_Moneda

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
                      AND (:FechaInicio IS NULL OR TRUNC(M.MOV_Fecha) >= :FechaInicio)
                      AND (:FechaFin IS NULL OR TRUNC(M.MOV_Fecha) <= :FechaFin)

                    ORDER BY M.MOV_Fecha ASC,
                             M.MOV_Movimiento ASC";

                var movimientos =
                    (await connection.QueryAsync<ReporteMovimientoDTO>(
                        sql,
                        new
                        {
                            CuentaId = cuentaId,
                            TipoMovimientoId = tipoMovimientoId,
                            MedioMovimientoId = medioMovimientoId,
                            EstadoMovimientoId = estadoMovimientoId,
                            PersonaId = personaId,
                            FechaInicio = fechaInicio?.Date,
                            FechaFin = fechaFin?.Date
                        },
                        transaction
                    )).ToList();

                decimal saldoInicial = 0;

                if (cuentaId.HasValue && fechaInicio.HasValue)
                {
                    var sqlSaldoInicial = @"
                        SELECT NVL((
                            SELECT M.MOV_Saldo
                            FROM GCB_MOVIMIENTO M
                            WHERE M.CUB_Cuenta = :CuentaId
                              AND TRUNC(M.MOV_Fecha) < :FechaInicio
                            ORDER BY M.MOV_Fecha DESC,
                                     M.MOV_Movimiento DESC
                            FETCH FIRST 1 ROWS ONLY
                        ), 0)
                        FROM DUAL";

                    saldoInicial =
                        await connection.ExecuteScalarAsync<decimal>(
                            sqlSaldoInicial,
                            new
                            {
                                CuentaId = cuentaId,
                                FechaInicio = fechaInicio.Value.Date
                            },
                            transaction
                        );
                }
                else if (movimientos.Any())
                {
                    var primerMovimiento = movimientos.First();

                    var esIngresoPrimerMovimiento =
                        primerMovimiento.TipoMovimiento
                            .Trim()
                            .ToLower()
                            .Contains("ingreso");

                    saldoInicial = esIngresoPrimerMovimiento
                        ? primerMovimiento.MOV_Saldo - primerMovimiento.MOV_Monto
                        : primerMovimiento.MOV_Saldo + primerMovimiento.MOV_Monto + primerMovimiento.MOV_Recargo;
                }

                decimal totalCreditos = movimientos
                    .Where(x => x.TipoMovimiento.Trim().ToLower().Contains("ingreso"))
                    .Sum(x => x.MOV_Monto);

                decimal totalDebitos = movimientos
                    .Where(x => !x.TipoMovimiento.Trim().ToLower().Contains("ingreso"))
                    .Sum(x => x.MOV_Monto);

                decimal totalRecargos = movimientos.Sum(x => x.MOV_Recargo);

                decimal saldoFinal = movimientos.Any()
                    ? movimientos.Last().MOV_Saldo
                    : saldoInicial;

                var resultado = new EstadoCuentaMovimientoDTO
                {
                    SaldoInicial = saldoInicial,
                    TotalCreditos = totalCreditos,
                    TotalDebitos = totalDebitos,
                    TotalRecargos = totalRecargos,
                    SaldoFinal = saldoFinal,
                    TotalMovimientos = movimientos.Count,
                    Movimientos = movimientos
                };

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
