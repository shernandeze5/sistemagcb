using GestionCuentasBancarias.Domain.DTOS.ReporteMovimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IReporteMovimientoRepository
    {
        Task<IEnumerable<ReporteMovimientoDTO>> ObtenerReporte(
            int? cuentaId,
            int? tipoMovimientoId,
            int? medioMovimientoId,
            int? estadoMovimientoId,
            int? personaId
        );
    }
}
