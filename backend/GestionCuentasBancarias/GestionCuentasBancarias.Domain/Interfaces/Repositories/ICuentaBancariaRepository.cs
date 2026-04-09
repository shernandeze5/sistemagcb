using GestionCuentasBancarias.Domain.DTOS.CuentaBancaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface ICuentaBancariaRepository
    {
        Task<List<ResponseCuentaBancariaDTO>> ObtenerCuentas();
        Task<List<ResponseCuentaBancariaDTO>> ObtenerCuentasPorBanco(int bancoId);
        Task<ResponseCuentaBancariaDTO> ObtenerCuentaPorId(int id);
        Task CrearCuenta(CreateCuentaBancariaDTO dto);
        Task<bool> ActualizarCuenta(int id, UpdateCuentaBancariaDTO dto);
        Task<bool> EliminarCuenta(int id);
        Task<bool> ReactivarCuenta(int id);
        Task<decimal> ObtenerSaldoActual(int cuentaId);
        Task ActualizarSaldoActual(int cuentaId, decimal nuevoSaldo);
    }
}
