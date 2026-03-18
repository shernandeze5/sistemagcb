using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.Entities;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IEstadoChequeRepository
    {
        Task<IEnumerable<EstadoCheque>> ObtenerTodosAsync();

        Task<EstadoCheque?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(EstadoCheque estadoCheque);

        Task<bool> ActualizarAsync(EstadoCheque estadoCheque);

        Task<bool> EliminarLogicoAsync(int id);
    }
}