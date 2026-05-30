using GestionCuentasBancarias.Domain.DTOS.ReporteConciliacion;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class ReporteConciliacionService : IReporteConciliacionService  
    {
        private readonly IReporteConciliacionRepository repository;

        public ReporteConciliacionService(IReporteConciliacionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ReporteConciliacionDTO>> ObtenerReporte(
            int? cuentaId,
            string? periodo,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int? estadoConciliacionId)
        {
            return await repository.ObtenerReporte(
                cuentaId,
                periodo,
                fechaInicio,
                fechaFin,
                estadoConciliacionId
            );
        }
    }
}
