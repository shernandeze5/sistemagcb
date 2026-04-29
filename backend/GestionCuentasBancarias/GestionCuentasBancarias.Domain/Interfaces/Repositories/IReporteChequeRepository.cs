using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOs.ReporteCheque;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IReporteChequeRepository
    {
        Task<IEnumerable<ReporteChequeDTO>> ObtenerReporte(
            int? cuentaId,
            int? estadoChequeId,
            int? chequeraId,
            int? personaId);
    }
}