using GestionCuentasBancarias.Domain.DTOS.Banco;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class BancoService : IBancoService
    {
        private readonly IBancoRepository repository;

        public BancoService(IBancoRepository repository)
        {
            this.repository = repository;
        }

        public Task<List<ResponseBancoDTO>> ObtenerBancos()
        {
            return repository.ObtenerBancos();
        }

        public Task<ResponseBancoDTO> ObtenerBancoPorId(int id)
        {
            return repository.ObtenerBancoPorId(id);
        }

        public Task CrearBanco(CreateBancoDTO dto)
        {
            return repository.CrearBanco(dto);
        }

        public Task<bool> ActualizarBanco(int id, UpdataBancoDTO dto)
        {
            return repository.ActualizarBanco(id, dto);
        }

        public Task<bool> EliminarBanco(int id)
        {
            return repository.EliminarBanco(id);
        }

        public Task<bool> ReactivarBanco(int id)
        {
            return repository.ReactivarBanco(id);
        }
    }
}
