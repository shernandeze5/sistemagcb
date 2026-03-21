using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.Entities;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface ITipoMovimientoRepository
    {
        Task<IEnumerable<TipoMovimiento>> ObtenerTodosAsync();
        Task<TipoMovimiento?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(TipoMovimiento tipoMovimiento);
        Task<bool> ActualizarAsync(TipoMovimiento tipoMovimiento);
        Task<bool> EliminarLogicoAsync(int id);
        Task<bool> Reactivar(int id);
    }
}
