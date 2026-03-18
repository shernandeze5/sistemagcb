using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class EstadoChequeService : IEstadoChequeService
    {
        private readonly IEstadoChequeRepository _repository;

        public EstadoChequeService(IEstadoChequeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EstadoChequeDTO>> ObtenerTodosAsync()
        {
            var est = await _repository.ObtenerTodosAsync();

            return est.Select(est => new EstadoChequeDTO
            {
                ESC_Estado_Cheque = est.ESC_Estado_Cheque,
                ESC_Descripcion = est.ESC_Descripcion,
                ESC_Estado = est.ESC_Estado,
                ESC_Fecha_Creacion = est.ESC_Fecha_Creacion
            });
        }

        public async Task<EstadoChequeDTO?> ObtenerPorIdAsync(int id)
        {
            var est = await _repository.ObtenerPorIdAsync(id);

            if (est == null)
                return null;

            return new EstadoChequeDTO
            {
                ESC_Estado_Cheque = est.ESC_Estado_Cheque,
                ESC_Descripcion = est.ESC_Descripcion,
                ESC_Estado = est.ESC_Estado,
                ESC_Fecha_Creacion = est.ESC_Fecha_Creacion
            };
        }

        public async Task<bool> CrearAsync(CrearEstadoChequeDTO dto)
        {
            var estado = new EstadoCheque
            {
                ESC_Descripcion = dto.ESC_Descripcion,
                ESC_Estado = 1,
                ESC_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(estado);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarEstadoChequeDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.ESC_Descripcion = dto.ESC_Descripcion;
            existente.ESC_Estado = dto.ESC_Estado;

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
