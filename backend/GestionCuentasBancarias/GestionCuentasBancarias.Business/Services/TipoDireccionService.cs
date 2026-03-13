using GestionCuentasBancarias.Domain.DTOS.TipoDireccion;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class TipoDireccionService : ITipoDireccionService
    {
        private readonly ITipoDireccionRepository _repository;

        public TipoDireccionService(ITipoDireccionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TipoDireccionDTO>> ObtenerTodosAsync()
        {
            var tipos = await _repository.ObtenerTodosAsync();

            return tipos.Select(t => new TipoDireccionDTO
            {
                TDI_Tipo_Direccion = t.TDI_Tipo_Direccion,
                TDI_Descripcion = t.TDI_Descripcion,
                TDI_Estado = t.TDI_Estado,
                TDI_Fecha_Creacion = t.TDI_Fecha_Creacion
            });
        }

        public async Task<bool> CrearAsync(CrearTipoDireccionDTO dto)
        {
            var entidad = new TipoDireccion
            {
                TDI_Descripcion = dto.TDI_Descripcion,
                TDI_Estado = "A",
                TDI_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarTipoDireccionDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.TDI_Descripcion = dto.TDI_Descripcion;
            existente.TDI_Estado = dto.TDI_Estado;

            return await _repository.ActualizarAsync(existente);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repository.EliminarAsync(id);
        }
    }
}
