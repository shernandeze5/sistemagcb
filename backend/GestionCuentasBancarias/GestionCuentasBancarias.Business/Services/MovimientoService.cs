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
    public class MovimientoService : IMovimientoService
    {
        private readonly IMovimientoRepository _repository;

        public MovimientoService(IMovimientoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MovimientoDTO>> ObtenerTodosAsync()
        {
            var movimientos = await _repository.ObtenerTodosAsync();

            return movimientos.Select(m => new MovimientoDTO
            {
                MOV_Movimiento = m.MOV_Movimiento,
                CUB_Cuenta = m.CUB_Cuenta,
                PER_Persona = m.PER_Persona,
                TIM_Tipo_Movimiento = m.TIM_Tipo_Movimiento,
                MEM_Medio_Movimiento = m.MEM_Medio_Movimiento,
                ESM_Estado_Movimiento = m.ESM_Estado_Movimiento,
                MOV_Fecha = m.MOV_Fecha,
                MOV_Numero_Referencia = m.MOV_Numero_Referencia,
                MOV_Descripcion = m.MOV_Descripcion,
                MOV_Monto = m.MOV_Monto,
                MOV_Saldo = m.MOV_Saldo
            });
        }

        public async Task<MovimientoDTO?> ObtenerPorIdAsync(int id)
        {
            var movimiento = await _repository.ObtenerPorIdAsync(id);

            if (movimiento == null)
                return null;

            return new MovimientoDTO
            {
                MOV_Movimiento = movimiento.MOV_Movimiento,
                CUB_Cuenta = movimiento.CUB_Cuenta,
                PER_Persona = movimiento.PER_Persona,
                TIM_Tipo_Movimiento = movimiento.TIM_Tipo_Movimiento,
                MEM_Medio_Movimiento = movimiento.MEM_Medio_Movimiento,
                ESM_Estado_Movimiento = movimiento.ESM_Estado_Movimiento,
                MOV_Fecha = movimiento.MOV_Fecha,
                MOV_Numero_Referencia = movimiento.MOV_Numero_Referencia,
                MOV_Descripcion = movimiento.MOV_Descripcion,
                MOV_Monto = movimiento.MOV_Monto,
                MOV_Saldo = movimiento.MOV_Saldo
            };
        }

        public async Task<bool> CrearAsync(CrearMovimientoDTO dto)
        {
            var entidad = new Movimiento
            {
                CUB_Cuenta = dto.CUB_Cuenta,
                PER_Persona = dto.PER_Persona,
                TIM_Tipo_Movimiento = dto.TIM_Tipo_Movimiento,
                MEM_Medio_Movimiento = dto.MEM_Medio_Movimiento,
                ESM_Estado_Movimiento = dto.ESM_Estado_Movimiento,
                MOV_Fecha = DateTime.Now,
                MOV_Numero_Referencia = dto.MOV_Numero_Referencia,
                MOV_Descripcion = dto.MOV_Descripcion,
                MOV_Monto = dto.MOV_Monto,
                MOV_Saldo = 0
            };

            return await _repository.CrearAsync(entidad);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarMovimientoDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.MOV_Descripcion = dto.MOV_Descripcion;
            existente.ESM_Estado_Movimiento = dto.ESM_Estado_Movimiento;

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