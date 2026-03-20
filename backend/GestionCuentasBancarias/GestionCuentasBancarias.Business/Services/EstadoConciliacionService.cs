using GestionCuentasBancarias.Domain.DTOS.Conciliacion;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class EstadoConciliacionService : IEstadoConciliacionService
    {
        private readonly IEstadoConciliacionRepository repository;

        public EstadoConciliacionService(IEstadoConciliacionRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<ResponseEstadoConciliacionDTO>> ObtenerEstadosConciliacion()
        {
            return repository.ObtenerEstadosConciliacion();
        }

        public Task<ResponseEstadoConciliacionDTO> ObtenerEstadoConciliacionPorId(int id)
        {
            return repository.ObtenerEstadoConciliacionPorId(id);
        }

        public Task<int> CrearEstadoConciliacion(CreateEstadoConciliacionDTO dto)
        {
            return repository.CrearEstadoConciliacion(dto);
        }

        public Task<bool> ActualizarEstadoConciliacion(int id, UpdateEstadoConciliacionDTO dto)
        {
            return repository.ActualizarEstadoConciliacion(id, dto);
        }

        public Task<bool> EliminarEstadoConciliacion(int id)
        {
            return repository.EliminarEstadoConciliacion(id);
        }

        public Task<bool> ReactivarEstadoConciliacion(int id)
        {
           return repository.ReactivarEstadoConciliacion(id);
        }
    }
}
