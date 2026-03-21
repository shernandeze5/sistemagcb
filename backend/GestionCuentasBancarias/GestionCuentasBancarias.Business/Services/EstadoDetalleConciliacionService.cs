using GestionCuentasBancarias.Domain.DTOS.DetalleConciliacion;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class EstadoDetalleConciliacionService : IEstadoDetalleConciliacionService
    {
        private readonly IEstadoDetalleConciliacionRepository repository;

        public EstadoDetalleConciliacionService(IEstadoDetalleConciliacionRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<ResponseEstadoDetalleConciliacionDTO>> ObtenerEstadosDetalleConciliacion()
        {
            return repository.ObtenerEstadosDetalleConciliacion();
        }

        public Task<ResponseEstadoDetalleConciliacionDTO> ObtenerEstadoDetalleConciliacionPorId(int id)
        {
            return repository.ObtenerEstadoDetalleConciliacionPorId(id);
        }

        public Task<int> CrearEstadoDetalleConciliacion(CreateEstadoDetalleConciliacionDTO dto)
        {
            return repository.CrearEstadoDetalleConciliacion(dto);
        }

        public Task<bool> ActualizarEstadoDetalleConciliacion(int id, UpdateEstadoDetalleConciliacionDTO dto)
        {
            return repository.ActualizarEstadoDetalleConciliacion(id, dto);
        }

        public Task<bool> EliminarEstadoDetalleConciliacion(int id)
        {
            return repository.EliminarEstadoDetalleConciliacion(id);
        }

        public Task<bool> ReactivarEstadoDetalleConciliacion(int id)
        {
           return repository.ReactivarEstadoDetalleConciliacion(id);
        }
    }
}
