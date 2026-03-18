using GestionCuentasBancarias.Domain.DTOS.EstadoCuenta;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class EstadoCuentaService : IEstadoCuentaService
    {
        private readonly IEstadoCuentaRepository repository;

        public EstadoCuentaService(IEstadoCuentaRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<ResponseEstadoCuentaDTO>> ObtenerEstadosCuenta()
        {
            return repository.ObtenerEstadosCuenta();
        }

        public Task<ResponseEstadoCuentaDTO> ObtenerEstadoCuentaPorId(int id)
        {
            return repository.ObtenerEstadoCuentaPorId(id);
        }

        public Task<int> CrearEstadoCuenta(CreateEstadoCuentaDTO dto)
        {
            return repository.CrearEstadoCuenta(dto);
        }

        public Task<bool> ActualizarEstadoCuenta(int id, UpdateEstadoCuentaDTO dto)
        {
            return repository.ActualizarEstadoCuenta(id, dto);
        }

        public Task<bool> EliminarEstadoCuenta(int id)
        {
            return repository.EliminarEstadoCuenta(id);
        }

        public Task<bool> ReactivarEstadoCuenta(int id)
        {
           return repository.ReactivarEstadoCuenta(id);
        }
    }
}
