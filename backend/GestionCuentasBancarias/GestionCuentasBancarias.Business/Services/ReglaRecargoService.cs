using GestionCuentasBancarias.Domain.DTOS.ReglaRecargo;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class ReglaRecargoService : IReglaRecargoService
    {
        private readonly IReglaRecargoRepository repository;

        public ReglaRecargoService(IReglaRecargoRepository repository)
        {
            this.repository = repository;
        }

        public Task<List<ResponseReglaRecargoDTO>> ObtenerReglas() =>
            repository.ObtenerReglas();

        public Task<List<ResponseReglaRecargoDTO>> ObtenerReglasPorCuenta(int cuentaId) =>
            repository.ObtenerReglasPorCuenta(cuentaId);

        public Task<ResponseReglaRecargoDTO> ObtenerReglaPorId(int id) =>
            repository.ObtenerReglaPorId(id);

        public Task CrearRegla(CreateReglaRecargoDTO dto) =>
            repository.CrearRegla(dto);

        public Task<bool> ActualizarRegla(int id, UpdateReglaRecargoDTO dto) =>
            repository.ActualizarRegla(id, dto);

        public Task<bool> EliminarRegla(int id) =>
            repository.EliminarRegla(id);

        public Task<bool> ReactivarRegla(int id) =>
            repository.ReactivarRegla(id);
    }
}
