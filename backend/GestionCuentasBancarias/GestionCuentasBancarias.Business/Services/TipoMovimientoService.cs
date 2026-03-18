using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Data.Repositories;
using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class TipoMovimientoService : ITipoMovimientoService
    {
        private readonly ITipoMovimientoRepository _repository;

        public TipoMovimientoService(ITipoMovimientoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TipoMovimientoDTO>> ObtenerTodosAsync()
        {
            var tipos = await _repository.ObtenerTodosAsync();

            return tipos.Select(t => new TipoMovimientoDTO
            {
                TIM_Tipo_Movimiento = t.TIM_Tipo_Movimiento,
                TIM_Descripcion = t.TIM_Descripcion,
                TIM_Estado = t.TIM_Estado,
                TIM_Fecha_Creacion = t.TIM_Fecha_Creacion
            });
        }

        public async Task<TipoMovimientoDTO?> ObtenerPorIdAsync(int id)
        {
            var tipo = await _repository.ObtenerPorIdAsync(id);

            if (tipo == null)
                return null;

            return new TipoMovimientoDTO
            {
                TIM_Tipo_Movimiento = tipo.TIM_Tipo_Movimiento,
                TIM_Descripcion = tipo.TIM_Descripcion,
                TIM_Estado = tipo.TIM_Estado,
                TIM_Fecha_Creacion = tipo.TIM_Fecha_Creacion
            };
        }

        public async Task<bool> CrearAsync(CrearTipoMovimientoDTO dto)
        {
            var entidad = new TipoMovimiento
            {
                TIM_Descripcion = dto.TIM_Descripcion,
                TIM_Estado = "A",
                TIM_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarTipoMovimientoDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.TIM_Descripcion = dto.TIM_Descripcion;
            existente.TIM_Estado = dto.TIM_Estado;

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