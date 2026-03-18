using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.DTOS.MedioMovimiento;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class MedioMovimientoService : IMedioMovimientoService
    {
        private readonly IMedioMovimientoRepository _repository;

        public MedioMovimientoService(IMedioMovimientoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MedioMovimientoDTO>> ObtenerTodosAsync()
        {
            var medios = await _repository.ObtenerTodosAsync();

            return medios.Select(m => new MedioMovimientoDTO
            {
                MEM_Medio_Movimiento = m.MEM_Medio_Movimiento,
                MEM_Descripcion = m.MEM_Descripcion,
                MEM_Estado = m.MEM_Estado,
                MEM_Fecha_Creacion = m.MEM_Fecha_Creacion
            });
        }

        public async Task<MedioMovimientoDTO?> ObtenerPorIdAsync(int id)
        {
            var medio = await _repository.ObtenerPorIdAsync(id);

            if (medio == null)
                return null;

            return new MedioMovimientoDTO
            {
                MEM_Medio_Movimiento = medio.MEM_Medio_Movimiento,
                MEM_Descripcion = medio.MEM_Descripcion,
                MEM_Estado = medio.MEM_Estado,
                MEM_Fecha_Creacion = medio.MEM_Fecha_Creacion
            };
        }

        public async Task<bool> CrearAsync(CrearMedioMovimientoDTO dto)
        {
            var entidad = new MedioMovimiento
            {
                MEM_Descripcion = dto.MEM_Descripcion,
                MEM_Estado = "A",
                MEM_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarMedioMovimientoDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.MEM_Descripcion = dto.MEM_Descripcion;
            existente.MEM_Estado = dto.MEM_Estado;

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
