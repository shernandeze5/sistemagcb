using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.Entities;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IMovimientoRepository
    {
        Task<IEnumerable<Movimiento>> ObtenerTodosAsync();

        Task<Movimiento?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(Movimiento movimiento);

        Task<bool> ActualizarAsync(Movimiento movimiento);

        Task<bool> EliminarLogicoAsync(int id);
    }
}
