using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.Conciliacion;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class EstadoConciliacionService : IEstadoConciliacionService
    {
        private readonly IEstadoConciliacionRepository _repository;

        public EstadoConciliacionService(IEstadoConciliacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EstadoConciliacionDTO>> ObtenerTodosAsync()
        {
            var estados = await _repository.ObtenerTodosAsync();

            return estados.Select(est => new EstadoConciliacionDTO
            {
                ECO_Estado_Conciliacion = est.ECO_Estado_Conciliacion,
                ECO_Descripcion = est.ECO_Descripcion,
                ECO_Estado = est.ECO_Estado,
                ECO_Fecha_Creacion = est.ECO_Fecha_Creacion
            });
        }

        public async Task<EstadoConciliacionDTO?> ObtenerPorIdAsync(int id)
        {
            var estc = await _repository.ObtenerPorIdAsync(id);

            if (estc == null)
                return null;

            return new EstadoConciliacionDTO
            {
                ECO_Estado_Conciliacion = estc.ECO_Estado_Conciliacion,
                ECO_Descripcion = estc.ECO_Descripcion,
                ECO_Estado = estc.ECO_Estado,
                ECO_Fecha_Creacion = estc.ECO_Fecha_Creacion
            };
        }

        public async Task<bool> CrearAsync(CrearEstadoConciliacionDTO dto)
        {
            var entidad = new EstadoConciliacion
            {
                ECO_Descripcion = dto.ECO_Descripcion,
                ECO_Estado = 1,
                ECO_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarEstadoConciliacionDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.ECO_Descripcion = dto.ECO_Descripcion;
            existente.ECO_Estado = dto.ECO_Estado;

            return await _repository.ActualizarAsync(existente);
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            return await _repository.EliminarLogicoAsync(id);
        }
    }
}
