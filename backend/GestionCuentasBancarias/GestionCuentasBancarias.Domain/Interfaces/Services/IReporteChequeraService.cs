using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOs.ReporteChequera;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IReporteChequeraService
    {
        Task<IEnumerable<ReporteChequeraDTO>> ObtenerReporte(
            int? cuentaId,
            string? estado);
    }
}