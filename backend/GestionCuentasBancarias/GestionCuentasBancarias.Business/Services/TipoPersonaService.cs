using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.DTOS.NewFolder;

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
    public class TipoPersonaService : ITipoPersonaService
    {
        private readonly ITipoPersonaRepository _repository;

        public TipoPersonaService(ITipoPersonaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TipoPersonaDTO>> ObtenerTodosAsync()
        {
            var tipos = await _repository.ObtenerTodosAsync();

            return tipos.Select(t => new TipoPersonaDTO
            {
                TIP_Tipo_Persona = t.TIP_Tipo_Persona,
                TIP_Descripcion = t.TIP_Descripcion,
                TIP_Estado = t.TIP_Estado,
                TIP_Fecha_Creacion = t.TIP_Fecha_Creacion
            });
        }

        public async Task<TipoPersonaDTO?> ObtenerPorIdAsync(int id)
        {
            var tipo = await _repository.ObtenerPorIdAsync(id);

            if (tipo == null)
                return null;

            return new TipoPersonaDTO
            {
                TIP_Tipo_Persona = tipo.TIP_Tipo_Persona,
                TIP_Descripcion = tipo.TIP_Descripcion,
                TIP_Estado = tipo.TIP_Estado,
                TIP_Fecha_Creacion = tipo.TIP_Fecha_Creacion
            };
        }

        public async Task<bool> CrearAsync(CrearTipoPersonaDTO dto)
        {
            var entidad = new TipoPersona
            {
                TIP_Descripcion = dto.TIP_Descripcion,
                TIP_Estado = dto.TIP_Estado,
                TIP_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarTipoPersonaDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.TIP_Descripcion = dto.TIP_Descripcion;
            existente.TIP_Estado = dto.TIP_Estado;

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
