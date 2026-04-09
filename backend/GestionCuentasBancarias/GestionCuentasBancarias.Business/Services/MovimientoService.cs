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
        private readonly IMovimientoRepository movRepo;
        private readonly ICuentaBancariaRepository cuentaRepo;

        public MovimientoService(
            IMovimientoRepository movRepo,
            ICuentaBancariaRepository cuentaRepo)
        {
            this.movRepo = movRepo;
            this.cuentaRepo = cuentaRepo;
        }

        // 🔥 CREAR MOVIMIENTO (CORREGIDO → DEVUELVE ID)
        public async Task<int> CrearMovimiento(CreateMovimientoDTO dto)
        {
            var saldoActual = await cuentaRepo.ObtenerSaldoActual(dto.CUB_Cuenta);
            decimal nuevoSaldo = saldoActual;

            switch (dto.TIM_Tipo_Movimiento)
            {
                case 1: // DEPÓSITO
                    nuevoSaldo += dto.MOV_Monto;
                    break;

                case 2: // RETIRO
                    if (saldoActual < dto.MOV_Monto)
                        throw new Exception("Saldo insuficiente");

                    nuevoSaldo -= dto.MOV_Monto;
                    break;

                case 3: // TRANSFERENCIA
                    if (dto.CUB_Cuenta_Destino == null)
                        throw new Exception("Cuenta destino requerida");

                    if (saldoActual < dto.MOV_Monto)
                        throw new Exception("Saldo insuficiente");

                    // Restar origen
                    nuevoSaldo -= dto.MOV_Monto;

                    // Sumar destino
                    var saldoDestino = await cuentaRepo.ObtenerSaldoActual(dto.CUB_Cuenta_Destino.Value);
                    await cuentaRepo.ActualizarSaldoActual(
                        dto.CUB_Cuenta_Destino.Value,
                        saldoDestino + dto.MOV_Monto
                    );
                    break;

                case 4: // PAGO
                    if (saldoActual < dto.MOV_Monto)
                        throw new Exception("Saldo insuficiente");

                    nuevoSaldo -= dto.MOV_Monto;
                    break;

                default:
                    throw new Exception("Tipo de movimiento inválido");
            }

            // 🔥 actualizar saldo origen
            await cuentaRepo.ActualizarSaldoActual(dto.CUB_Cuenta, nuevoSaldo);

            dto.MOV_Saldo = nuevoSaldo;

            // 🔥 guardar y devolver ID
            return await movRepo.CrearMovimiento(dto);
        }

        // 🔍 OBTENER POR ID
        public async Task<ResponseMovimientoDTO?> ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Id inválido");

            return await movRepo.ObtenerPorId(id);
        }

        // 📄 OBTENER POR CUENTA
        public async Task<IEnumerable<ResponseMovimientoDTO>> ObtenerPorCuenta(int cuentaId)
        {
            if (cuentaId <= 0)
                throw new InvalidOperationException("Cuenta inválida");

            return await movRepo.ObtenerPorCuenta(cuentaId);
        }
    }
}
