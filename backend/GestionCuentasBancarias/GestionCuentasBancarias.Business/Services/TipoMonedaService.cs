using GestionCuentasBancarias.Domain.Interfaces.Services;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.TipoMoneda;

namespace GestionCuentasBancarias.Business.Services
{
    public class TipoMonedaService : ITipoMonedaService
    {
        private readonly ITipoMonedaRepository repository;

        public TipoMonedaService(ITipoMonedaRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<ResponseTipoMonedaDTO>> ObtenerTiposMoneda()
        {
            return repository.ObtenerTiposMoneda();
        }

        public Task<ResponseTipoMonedaDTO> ObtenerTipoMonedaPorId(int id)
        {
            return repository.ObtenerTipoMonedaPorId(id);
        }

        public Task<int> CrearTipoMoneda(CreateTipoMonedaDTO dto)
        {
            return repository.CrearTipoMoneda(dto);
        }

        public Task<bool> ActualizarTipoMoneda(int id, UpdateTipoMonedaDTO dto)
        {
            return repository.ActualizarTipoMoneda(id, dto);
        }

        public Task<bool> EliminarTipoMoneda(int id)
        {
            return repository.EliminarTipoMoneda(id);
        }

        public Task<bool> ReactivarTipoMoneda(int id)
        {
            return repository.ReactivarTipoMoneda(id);
        }
    }
}
