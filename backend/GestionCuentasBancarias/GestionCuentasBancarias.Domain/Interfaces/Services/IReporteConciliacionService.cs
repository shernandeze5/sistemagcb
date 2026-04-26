using GestionCuentasBancarias.Domain.DTOS.ReporteConciliacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IReporteConciliacionService
    {
        Task<IEnumerable<ReporteConciliacionDTO>> ObtenerReporte(
            int? cuentaId,
            string? periodo,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int? estadoConciliacionId
        );
    }
}
