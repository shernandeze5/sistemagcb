using GestionCuentasBancarias.Data.Repositories;
using GestionCuentasBancarias.Domain.DTOS.InteresFrecuencia;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class InteresFrecuenciaService : IinteresFrecuenciaService
    {
        private readonly IinteresFrecuenciaRepository repository;

        public InteresFrecuenciaService(IinteresFrecuenciaRepository repository)
        {
            this.repository = repository;
        }

        public Task<List<ResponseInteresFrecuenciaDTO>> ObtenerFrecuencias() =>
            repository.ObtenerFrecuencias();

        public Task<ResponseInteresFrecuenciaDTO> ObtenerFrecuenciaPorId(int id) =>
            repository.ObtenerFrecuenciaPorId(id);

        public Task CrearFrecuencia(CreateInteresFrecuenciaDTO dto) =>
            repository.CrearFrecuencia(dto);

        public Task<bool> ActualizarFrecuencia(int id, UpdateInteresFrecuenciaDTO dto) =>
            repository.ActualizarFrecuencia(id, dto);

        public Task<bool> EliminarFrecuencia(int id) =>
            repository.EliminarFrecuencia(id);

        public Task<bool> ReactivarFrecuencia(int id) =>
            repository.ReactivarFrecuencia(id);
    }
}
