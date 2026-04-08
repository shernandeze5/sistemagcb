using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class ChequeService : IChequeService
    {
        private readonly IChequeRepository _repository;
        private readonly IChequeraRepository _chequeraRepository;

        public ChequeService(
            IChequeRepository repository,
            IChequeraRepository chequeraRepository)
        {
            _repository = repository;
            _chequeraRepository = chequeraRepository;
        }

        public async Task<IEnumerable<ResponseChequeDTO>> ObtenerTodosAsync()
        {
            var cheques = await _repository.ObtenerTodosAsync();

            return cheques.Select(c => new ResponseChequeDTO
            {
                CHE_Cheque = c.CHE_Cheque,
                MOV_Movimiento = c.MOV_Movimiento,
                CHE_Numero_Cheque = c.CHE_Numero_Cheque,
                CHE_Monto_Letras = c.CHE_Monto_Letras,
                CHE_Fecha_Emision = c.CHE_Fecha_Emision,
                CHE_Fecha_Cobro = c.CHE_Fecha_Cobro,
                CHE_Fecha_Vencimiento = c.CHE_Fecha_Vencimiento,
                ESC_Estado_Cheque = c.ESC_Estado_Cheque,
                CHE_Fecha_Creacion = c.CHE_Fecha_Creacion,
                CHQ_Chequera = c.CHQ_Chequera,
                CHE_Beneficiario = c.CHE_Beneficiario,
                CHE_Concepto = c.CHE_Concepto
            });
        }

        public async Task<ResponseChequeDTO?> ObtenerPorIdAsync(int id)
        {
            var cheque = await _repository.ObtenerPorIdAsync(id);

            if (cheque == null)
                return null;

            return new ResponseChequeDTO
            {
                CHE_Cheque = cheque.CHE_Cheque,
                MOV_Movimiento = cheque.MOV_Movimiento,
                CHE_Numero_Cheque = cheque.CHE_Numero_Cheque,
                CHE_Monto_Letras = cheque.CHE_Monto_Letras,
                CHE_Fecha_Emision = cheque.CHE_Fecha_Emision,
                CHE_Fecha_Cobro = cheque.CHE_Fecha_Cobro,
                CHE_Fecha_Vencimiento = cheque.CHE_Fecha_Vencimiento,
                ESC_Estado_Cheque = cheque.ESC_Estado_Cheque,
                CHE_Fecha_Creacion = cheque.CHE_Fecha_Creacion,
                CHQ_Chequera = cheque.CHQ_Chequera,
                CHE_Beneficiario = cheque.CHE_Beneficiario,
                CHE_Concepto = cheque.CHE_Concepto
            };
        }

        public async Task<bool> CrearAsync(CrearChequeDTO dto)
        {
            if (!dto.CHQ_Chequera.HasValue || dto.CHQ_Chequera.Value <= 0)
                return false;

            var chequera = await _chequeraRepository.ObtenerPorIdAsync(dto.CHQ_Chequera.Value);

            if (chequera == null)
                return false;

            if (chequera.CHQ_Estado != "Activa")
                return false;

            int siguienteNumero;

            if (chequera.CHQ_Ultimo_Usado <= 0)
                siguienteNumero = chequera.CHQ_Numero_Desde;
            else
                siguienteNumero = chequera.CHQ_Ultimo_Usado + 1;

            if (siguienteNumero > chequera.CHQ_Numero_Hasta)
            {
                chequera.CHQ_Estado = "Agotada";
                await _chequeraRepository.ActualizarAsync(chequera);
                return false;
            }

            var entidad = new Cheque
            {
                MOV_Movimiento = dto.MOV_Movimiento,
                CHE_Numero_Cheque = dto.CHE_Numero_Cheque,
                CHE_Monto_Letras = dto.CHE_Monto_Letras,
                CHE_Fecha_Emision = dto.CHE_Fecha_Emision,
                CHE_Fecha_Cobro = dto.CHE_Fecha_Cobro,
                CHE_Fecha_Vencimiento = dto.CHE_Fecha_Vencimiento,
                ESC_Estado_Cheque = dto.ESC_Estado_Cheque,
                CHE_Fecha_Creacion = DateTime.Now,
                CHQ_Chequera = dto.CHQ_Chequera,
                CHE_Beneficiario = dto.CHE_Beneficiario,
                CHE_Concepto = dto.CHE_Concepto
            };

            var creado = await _repository.CrearAsync(entidad);

            if (!creado)
                return false;

            chequera.CHQ_Ultimo_Usado = siguienteNumero;

            if (chequera.CHQ_Ultimo_Usado >= chequera.CHQ_Numero_Hasta)
                chequera.CHQ_Estado = "Agotada";

            var actualizada = await _chequeraRepository.ActualizarAsync(chequera);

            return actualizada;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarChequeDTO dto)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            existente.MOV_Movimiento = dto.MOV_Movimiento;
            existente.CHE_Numero_Cheque = dto.CHE_Numero_Cheque;
            existente.CHE_Monto_Letras = dto.CHE_Monto_Letras;
            existente.CHE_Fecha_Emision = dto.CHE_Fecha_Emision;
            existente.CHE_Fecha_Cobro = dto.CHE_Fecha_Cobro;
            existente.CHE_Fecha_Vencimiento = dto.CHE_Fecha_Vencimiento;
            existente.ESC_Estado_Cheque = dto.ESC_Estado_Cheque;
            existente.CHQ_Chequera = dto.CHQ_Chequera;
            existente.CHE_Beneficiario = dto.CHE_Beneficiario;
            existente.CHE_Concepto = dto.CHE_Concepto;

            return await _repository.ActualizarAsync(existente);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            return await _repository.EliminarAsync(id);
        }
    }
}