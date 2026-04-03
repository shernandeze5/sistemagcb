using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.Entities;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IChequeRepository
    {
        Task<IEnumerable<Cheque>> ObtenerTodosAsync();
        Task<Cheque?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(Cheque cheque);
        Task<bool> ActualizarAsync(Cheque cheque);
        Task<bool> EliminarAsync(int id);
    }
}