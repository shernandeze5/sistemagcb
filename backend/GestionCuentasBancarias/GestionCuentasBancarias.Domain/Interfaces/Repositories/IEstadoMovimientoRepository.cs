using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.Entities;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IEstadoMovimientoRepository
    {
        Task<IEnumerable<EstadoMovimiento>> ObtenerTodosAsync();

        Task<EstadoMovimiento?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(EstadoMovimiento estadoMovimiento);

        Task<bool> ActualizarAsync(EstadoMovimiento estadoMovimiento);

        Task<bool> EliminarLogicoAsync(int id);
    }
}
