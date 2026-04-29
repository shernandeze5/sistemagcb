using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOs.ReporteChequera;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class ReporteChequeraService : IReporteChequeraService
    {
        private readonly IReporteChequeraRepository repository;

        public ReporteChequeraService(IReporteChequeraRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ReporteChequeraDTO>> ObtenerReporte(
            int? cuentaId,
            string? estado)
        {
            return await repository.ObtenerReporte(cuentaId, estado);
        }
    }
}