using GestionCuentasBancarias.Domain.DTOS.ReporteCuentaBancaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IReporteCuentaBancariaService
    {
        Task<IEnumerable<ReporteCuentaBancariaDTO>> ObtenerReporte(
            int? bancoId,
            int? tipoCuentaId,
            int? tipoMonedaId,
            int? estadoCuentaId,
            string? estadoRegistro
        );
    }
}
