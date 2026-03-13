using GestionCuentasBancarias.Domain.DTOS.TipoTelefono;
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
    public class TipoTelefonoService : ITipoTelefonoService
    {
        private readonly ITipoTelefonoRepository _repository;

        public TipoTelefonoService(ITipoTelefonoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TipoTelefonoDTO>> ObtenerTodosAsync()
        {
            var tipos = await _repository.ObtenerTodosAsync();

            return tipos.Select(t => new TipoTelefonoDTO
            {
                TIT_Tipo_Telefono = t.TIT_Tipo_Telefono,
                TIT_Descripcion = t.TIT_Descripcion,
                TIT_Estado = t.TIT_Estado,
                TIT_Fecha_Creacion = t.TIT_Fecha_Creacion
            });
        }

        public async Task<bool> CrearAsync(CrearTipoTelefonoDTO dto)
        {
            var entidad = new TipoTelefono
            {
                TIT_Descripcion = dto.TIT_Descripcion,
                TIT_Estado = "A",
                TIT_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarTipoTelefonoDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.TIT_Descripcion = dto.TIT_Descripcion;
            existente.TIT_Estado = dto.TIT_Estado;

            return await _repository.ActualizarAsync(existente);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repository.EliminarAsync(id);
        }
    }
}
