using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class EstadoMovimientoService : IEstadoMovimientoService
    {
        private readonly IEstadoMovimientoRepository _repository;

        public EstadoMovimientoService(IEstadoMovimientoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EstadoMovimientoDTO>> ObtenerTodosAsync()
        {
            var estados = await _repository.ObtenerTodosAsync();

            return estados.Select(e => new EstadoMovimientoDTO
            {
                ESM_Estado_Movimiento = e.ESM_Estado_Movimiento,
                ESM_Descripcion = e.ESM_Descripcion,
                ESM_Estado = e.ESM_Estado,
                ESM_Fecha_Creacion = e.ESM_Fecha_Creacion
            });
        }

        public async Task<EstadoMovimientoDTO?> ObtenerPorIdAsync(int id)
        {
            var estado = await _repository.ObtenerPorIdAsync(id);

            if (estado == null)
                return null;

            return new EstadoMovimientoDTO
            {
                ESM_Estado_Movimiento = estado.ESM_Estado_Movimiento,
                ESM_Descripcion = estado.ESM_Descripcion,
                ESM_Estado = estado.ESM_Estado,
                ESM_Fecha_Creacion = estado.ESM_Fecha_Creacion
            };
        }

        public async Task<bool> CrearAsync(CrearEstadoMovimientoDTO dto)
        {
            var entidad = new EstadoMovimiento
            {
                ESM_Descripcion = dto.ESM_Descripcion,
                ESM_Estado = "A",
                ESM_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarEstadoMovimientoDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.ESM_Descripcion = dto.ESM_Descripcion;
            existente.ESM_Estado = dto.ESM_Estado;

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
