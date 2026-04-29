using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOs.ReporteCheque;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IReporteChequeService
    {
        Task<IEnumerable<ReporteChequeDTO>> ObtenerReporte(
            int? cuentaId,
            int? estadoChequeId,
            int? chequeraId,
            int? personaId);
    }
}