using GestionCuentasBancarias.Domain.DTOS.ReporteMovimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IReporteMovimientoService
    {
        Task<EstadoCuentaMovimientoDTO> ObtenerReporte(
            int? cuentaId,
            int? tipoMovimientoId,
            int? medioMovimientoId,
            int? estadoMovimientoId,
            int? personaId,
            DateTime? fechaInicio,
            DateTime? fechaFin);
    }
}
