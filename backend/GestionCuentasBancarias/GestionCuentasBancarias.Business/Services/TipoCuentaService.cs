using GestionCuentasBancarias.Domain.DTOS.TipoCuenta;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class TipoCuentaService : ITipoCuentaService
    {
        private readonly ITipoCuentaRepository repository;

        public TipoCuentaService(ITipoCuentaRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<ResponseTipoCuentaDTO>> ObtenerTiposCuenta()
        {
            return repository.ObtenerTiposCuenta();
        }

        public Task<ResponseTipoCuentaDTO> ObtenerTipoCuentaPorId(int id)
        {
            return repository.ObtenerTipoCuentaPorId(id);
        }

        public Task<int> CrearTipoCuenta(CreateTipoCuentaDTO dto)
        {
            return repository.CrearTipoCuenta(dto);
        }

        public Task<bool> ActualizarTipoCuenta(int id, UpdateTipoCuentaDTO dto)
        {
            return repository.ActualizarTipoCuenta(id, dto);
        }

        public Task<bool> EliminarTipoCuenta(int id)
        {
            return repository.EliminarTipoCuenta(id);
        }

        public Task<bool> ReactivarTipoCuenta(int id)
        {
            return repository.ReactivarTipoCuenta(id);
        }     
    }
}
