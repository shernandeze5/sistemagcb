using GestionCuentasBancarias.Domain.DTOS.TasaInteres;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class TasaInteresService : ITasaInteresService
    {
        private readonly ITasaInteresRepository repository;

        public TasaInteresService(ITasaInteresRepository repository)
        {
            this.repository = repository;
        }

        public Task<List<ResponseTasaInteresDTO>> ObtenerTasas() =>
            repository.ObtenerTasas();

        public Task<ResponseTasaInteresDTO> ObtenerTasaPorId(int id) =>
            repository.ObtenerTasaPorId(id);

        public Task CrearTasa(CreateTasaInteresDTO dto) =>
            repository.CrearTasa(dto);

        public Task<bool> ActualizarTasa(int id, UpdateTasaInteresDTO dto) =>
            repository.ActualizarTasa(id, dto);

        public Task<bool> EliminarTasa(int id) =>
            repository.EliminarTasa(id);

        public Task<bool> ReactivarTasa(int id) =>
            repository.ReactivarTasa(id);
    }
}
