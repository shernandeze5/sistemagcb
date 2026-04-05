using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.Entities;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IChequeraRepository
    {
        Task<IEnumerable<Chequera>> ObtenerTodosAsync();
        Task<Chequera?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(Chequera chequera);
        Task<bool> ActualizarAsync(Chequera chequera);
        Task<bool> EliminarLogicoAsync(int id);
        Task<bool> Reactivar(int id);
    }
}