using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.Entities;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IMedioMovimientoRepository
    {
        Task<IEnumerable<MedioMovimiento>> ObtenerTodosAsync();

        Task<MedioMovimiento?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(MedioMovimiento medioMovimiento);

        Task<bool> ActualizarAsync(MedioMovimiento medioMovimiento);

        Task<bool> EliminarLogicoAsync(int id);
    }
}