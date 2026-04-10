using GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IConciliacionRepository
    {
        Task<bool> ExisteCuentaActiva(int cuentaId);
        Task<bool> ExisteConciliacionPeriodo(int cuentaId, string periodo);

        Task<IEnumerable<MovimientoSistemaConciliacionDTO>> ObtenerMovimientosSistema(
            int cuentaId,
            DateTime fechaInicio,
            DateTime fechaFin);

        Task<decimal> ObtenerSaldoLibrosAlCorte(int cuentaId, DateTime fechaCorte);

        Task<int> ObtenerEstadoConciliacionId(string descripcion);
        Task<int> ObtenerEstadoDetalleId(string descripcion);
        Task<int> ObtenerTipoMovimientoId(string descripcion);
        Task<int> ObtenerMedioMovimientoId(string descripcion);
        Task<int> ObtenerEstadoMovimientoId(string descripcion);

        Task<int> GuardarProcesoConciliacion(GuardarConciliacionDTO dto);

        Task<ConciliacionResponseDTO?> ObtenerPorId(int conciliacionId);
        Task<IEnumerable<DetalleConciliacionResponseDTO>> ObtenerDetalle(int conciliacionId);
        Task<IEnumerable<ConciliacionResponseDTO>> ObtenerPorCuenta(int cuentaId);
        Task<IEnumerable<ConciliacionResponseDTO>> ObtenerTodas();

        Task<DetalleConciliacionContextDTO?> ObtenerDetalleContexto(int detalleId);
        Task ActualizarEstadoDetalle(int detalleId, int estadoDetalleId);
        Task ActualizarEstadoConciliacion(int conciliacionId, int estadoConciliacionId);

        Task<bool> ExisteMovimientoCuentaPorReferenciaMonto(int cuentaId, string referencia, decimal monto);
        Task<int> CrearMovimientoDesdeBanco(
            int cuentaId,
            DateTime fecha,
            string? referencia,
            string descripcion,
            decimal monto,
            int tipoMovimientoId,
            int medioMovimientoId,
            int estadoMovimientoId);

        Task<decimal> ObtenerSaldoActualCuenta(int cuentaId);
    }
}

