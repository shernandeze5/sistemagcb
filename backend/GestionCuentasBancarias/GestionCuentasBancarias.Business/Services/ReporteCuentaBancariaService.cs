using GestionCuentasBancarias.Domain.DTOS.ReporteCuentaBancaria;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class ReporteCuentaBancariaService : IReporteCuentaBancariaService
    {
        private readonly IReporteCuentaBancariaRepository repository;

        public ReporteCuentaBancariaService(IReporteCuentaBancariaRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ReporteCuentaBancariaDTO>> ObtenerReporte(
            int? bancoId,
            int? tipoCuentaId,
            int? tipoMonedaId,
            int? estadoCuentaId,
            string? estadoRegistro)
        {
            return await repository.ObtenerReporte(
                bancoId,
                tipoCuentaId,
                tipoMonedaId,
                estadoCuentaId,
                estadoRegistro
            );
        }
    }
}
