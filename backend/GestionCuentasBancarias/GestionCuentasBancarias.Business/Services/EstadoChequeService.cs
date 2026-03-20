using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class EstadoChequeService : IEstadoChequeService
    {
        private readonly IEstadoChequeRepository repository;

        public EstadoChequeService(IEstadoChequeRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<ResponseEstadoChequeDTO>> ObtenerEstadosCheque()
        {
            return repository.ObtenerEstadosCheque();
        }

        public Task<ResponseEstadoChequeDTO> ObtenerEstadoChequePorId(int id)
        {
            return repository.ObtenerEstadoChequePorId(id);
        }

        public Task<int> CrearEstadoCheque(CreateEstadoChequeDTO dto)
        {
            return repository.CrearEstadoCheque(dto);
        }

        public Task<bool> ActualizarEstadoCheque(int id, UpdateEstadoChequeDTO dto)
        {
            return repository.ActualizarEstadoCheque(id, dto);
        }

        public Task<bool> EliminarEstadoCheque(int id)
        {
            return repository.EliminarEstadoCheque(id);
        }

        public Task<bool> ReactivarEstadoCheque(int id)
        {
           return repository.ReactivarEstadoCheque(id);
        }
    }
}
