using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.DetalleConciliacion;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class EstadoDetalleConciliacionService : IEstadoDetalleConciliacionService
    {
        private readonly IEstadoDetalleConciliacionRepository _repository;

        public EstadoDetalleConciliacionService(IEstadoDetalleConciliacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EstadoDetalleConciliacionDTO>> ObtenerTodosAsync()
        {
            var estados = await _repository.ObtenerTodosAsync();

            return estados.Select(e => new EstadoDetalleConciliacionDTO
            {
                EDC_Estado_Detalle_Conciliacion = e.EDC_Estado_Detalle_Conciliacion,
                EDC_Descripcion = e.EDC_Descripcion,
                EDC_Estado = e.EDC_Estado,
                EDC_Fecha_Creacion = e.EDC_Fecha_Creacion
            });
        }

        public async Task<EstadoDetalleConciliacionDTO?> ObtenerPorIdAsync(int id)
        {
            var estado = await _repository.ObtenerPorIdAsync(id);

            if (estado == null)
                return null;

            return new EstadoDetalleConciliacionDTO
            {
                EDC_Estado_Detalle_Conciliacion = estado.EDC_Estado_Detalle_Conciliacion,
                EDC_Descripcion = estado.EDC_Descripcion,
                EDC_Estado = estado.EDC_Estado,
                EDC_Fecha_Creacion = estado.EDC_Fecha_Creacion
            };
        }

        public async Task<bool> CrearAsync(CrearEstadoDetalleConciliacionDTO dto)
        {
            var entidad = new EstadoDetalleConciliacion
            {
                EDC_Descripcion = dto.EDC_Descripcion,
                EDC_Estado = 1,
                EDC_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarEstadoDetalleConciliacionDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.EDC_Descripcion = dto.EDC_Descripcion;
            existente.EDC_Estado = dto.EDC_Estado;

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
