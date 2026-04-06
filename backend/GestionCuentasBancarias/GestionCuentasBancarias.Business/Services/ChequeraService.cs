using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.Chequera;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class ChequeraService : IChequeraService
    {
        private readonly IChequeraRepository _repository;

        public ChequeraService(IChequeraRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ChequeraDTO>> ObtenerTodosAsync()
        {
            var chequeras = await _repository.ObtenerTodosAsync();

            return chequeras.Select(c => new ChequeraDTO
            {
                CHQ_Chequera = c.CHQ_Chequera,
                CUB_Cuenta = c.CUB_Cuenta,
                CHQ_Serie = c.CHQ_Serie,
                CHQ_Numero_Desde = c.CHQ_Numero_Desde,
                CHQ_Numero_Hasta = c.CHQ_Numero_Hasta,
                CHQ_Ultimo_Usado = c.CHQ_Ultimo_Usado,
                CHQ_Estado = c.CHQ_Estado,
                CHQ_Fecha_Recepcion = c.CHQ_Fecha_Recepcion,
                CHQ_Fecha_Creacion = c.CHQ_Fecha_Creacion
            });
        }

        public async Task<ChequeraDTO?> ObtenerPorIdAsync(int id)
        {
            var chequera = await _repository.ObtenerPorIdAsync(id);

            if (chequera == null)
                return null;

            return new ChequeraDTO
            {
                CHQ_Chequera = chequera.CHQ_Chequera,
                CUB_Cuenta = chequera.CUB_Cuenta,
                CHQ_Serie = chequera.CHQ_Serie,
                CHQ_Numero_Desde = chequera.CHQ_Numero_Desde,
                CHQ_Numero_Hasta = chequera.CHQ_Numero_Hasta,
                CHQ_Ultimo_Usado = chequera.CHQ_Ultimo_Usado,
                CHQ_Estado = chequera.CHQ_Estado,
                CHQ_Fecha_Recepcion = chequera.CHQ_Fecha_Recepcion,
                CHQ_Fecha_Creacion = chequera.CHQ_Fecha_Creacion
            };
        }

        public async Task<bool> CrearAsync(CrearChequeraDTO dto)
        {
            ValidarCrear(dto);

            var entidad = new Chequera
            {
                CUB_Cuenta = dto.CUB_Cuenta,
                CHQ_Serie = dto.CHQ_Serie.Trim(),
                CHQ_Numero_Desde = dto.CHQ_Numero_Desde,
                CHQ_Numero_Hasta = dto.CHQ_Numero_Hasta,
                CHQ_Ultimo_Usado = 0,
                CHQ_Estado = "Pendiente",
                CHQ_Fecha_Recepcion = dto.CHQ_Fecha_Recepcion,
                CHQ_Fecha_Creacion = DateTime.Now
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarChequeraDTO dto)
        {
            ValidarActualizar(dto);

            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.CUB_Cuenta = dto.CUB_Cuenta;
            existente.CHQ_Serie = dto.CHQ_Serie.Trim();
            existente.CHQ_Numero_Desde = dto.CHQ_Numero_Desde;
            existente.CHQ_Numero_Hasta = dto.CHQ_Numero_Hasta;
            existente.CHQ_Ultimo_Usado = dto.CHQ_Ultimo_Usado;
            existente.CHQ_Estado = dto.CHQ_Estado.Trim();
            existente.CHQ_Fecha_Recepcion = dto.CHQ_Fecha_Recepcion;

            return await _repository.ActualizarAsync(existente);
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            return await _repository.EliminarLogicoAsync(id);
        }

        public async Task<bool> Reactivar(int id)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            return await _repository.Reactivar(id);
        }

        private void ValidarCrear(CrearChequeraDTO dto)
        {
            if (dto.CUB_Cuenta <= 0)
                throw new ArgumentException("La cuenta es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.CHQ_Serie))
                throw new ArgumentException("La serie es obligatoria.");

            if (dto.CHQ_Serie.Trim().Length > 5)
                throw new ArgumentException("La serie no puede tener más de 5 caracteres.");

            if (dto.CHQ_Numero_Desde <= 0)
                throw new ArgumentException("El número inicial debe ser mayor que cero.");

            if (dto.CHQ_Numero_Hasta <= 0)
                throw new ArgumentException("El número final debe ser mayor que cero.");

            if (dto.CHQ_Numero_Hasta < dto.CHQ_Numero_Desde)
                throw new ArgumentException("El número hasta no puede ser menor al número desde.");

            if (dto.CHQ_Fecha_Recepcion == default)
                throw new ArgumentException("La fecha de recepción es obligatoria.");
        }

        private void ValidarActualizar(ActualizarChequeraDTO dto)
        {
            if (dto.CUB_Cuenta <= 0)
                throw new ArgumentException("La cuenta es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.CHQ_Serie))
                throw new ArgumentException("La serie es obligatoria.");

            if (dto.CHQ_Serie.Trim().Length > 5)
                throw new ArgumentException("La serie no puede tener más de 5 caracteres.");

            if (dto.CHQ_Numero_Desde <= 0)
                throw new ArgumentException("El número inicial debe ser mayor que cero.");

            if (dto.CHQ_Numero_Hasta <= 0)
                throw new ArgumentException("El número final debe ser mayor que cero.");

            if (dto.CHQ_Numero_Hasta < dto.CHQ_Numero_Desde)
                throw new ArgumentException("El número hasta no puede ser menor al número desde.");

            if (dto.CHQ_Ultimo_Usado < 0)
                throw new ArgumentException("El último usado no puede ser negativo.");

            if (dto.CHQ_Ultimo_Usado > dto.CHQ_Numero_Hasta)
                throw new ArgumentException("El último usado no puede ser mayor al número hasta.");

            if (string.IsNullOrWhiteSpace(dto.CHQ_Estado))
                throw new ArgumentException("El estado es obligatorio.");

            var estadosValidos = new[] { "Pendiente", "Activa", "Agotada", "Anulada" };
            if (!estadosValidos.Contains(dto.CHQ_Estado.Trim()))
                throw new ArgumentException("El estado no es válido.");

            if (dto.CHQ_Fecha_Recepcion == default)
                throw new ArgumentException("La fecha de recepción es obligatoria.");
        }
    }
}