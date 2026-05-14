using GestionCuentasBancarias.Domain.DTOS.ReporteMovimiento;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class ReporteMovimientoService : IReporteMovimientoService
    {
        private readonly IReporteMovimientoRepository repository;

        public ReporteMovimientoService(IReporteMovimientoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<EstadoCuentaMovimientoDTO> ObtenerReporte(
           int? cuentaId,
           int? tipoMovimientoId,
           int? medioMovimientoId,
           int? estadoMovimientoId,
           int? personaId,
           DateTime? fechaInicio,
           DateTime? fechaFin)
        {
            return await repository.ObtenerReporte(
                cuentaId,
                tipoMovimientoId,
                medioMovimientoId,
                estadoMovimientoId,
                personaId,
                fechaInicio,
                fechaFin
            );
        }
    }
}
