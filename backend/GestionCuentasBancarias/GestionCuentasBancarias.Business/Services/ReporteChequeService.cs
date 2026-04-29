using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOs.ReporteCheque;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class ReporteChequeService : IReporteChequeService
    {
        private readonly IReporteChequeRepository repository;

        public ReporteChequeService(IReporteChequeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ReporteChequeDTO>> ObtenerReporte(
            int? cuentaId,
            int? estadoChequeId,
            int? chequeraId,
            int? personaId)
        {
            return await repository.ObtenerReporte(cuentaId, estadoChequeId, chequeraId, personaId);
        }
    }
}