using GestionCuentasBancarias.Domain.DTOS.CuentaBancaria;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class CuentaBancariaService : ICuentaBancariaService
    {
        private readonly ICuentaBancariaRepository repository;

        public CuentaBancariaService(ICuentaBancariaRepository repository)
        {
            this.repository = repository;
        }

        public Task<List<ResponseCuentaBancariaDTO>> ObtenerCuentas() =>
            repository.ObtenerCuentas();

        public Task<List<ResponseCuentaBancariaDTO>> ObtenerCuentasPorBanco(int bancoId) =>
            repository.ObtenerCuentasPorBanco(bancoId);

        public Task<ResponseCuentaBancariaDTO> ObtenerCuentaPorId(int id) =>
            repository.ObtenerCuentaPorId(id);

        public Task CrearCuenta(CreateCuentaBancariaDTO dto) =>
            repository.CrearCuenta(dto);

        public Task<bool> ActualizarCuenta(int id, UpdateCuentaBancariaDTO dto) =>
            repository.ActualizarCuenta(id, dto);

        public Task<bool> EliminarCuenta(int id) =>
            repository.EliminarCuenta(id);

        public Task<bool> ReactivarCuenta(int id) =>
            repository.ReactivarCuenta(id);
    }
}
