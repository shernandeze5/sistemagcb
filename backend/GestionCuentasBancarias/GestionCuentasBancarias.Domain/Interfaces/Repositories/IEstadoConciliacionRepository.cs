using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.Entities;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IEstadoConciliacionRepository
    {
        Task<IEnumerable<EstadoConciliacion>> ObtenerTodosAsync();

        Task<EstadoConciliacion?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(EstadoConciliacion estadoConciliacion);

        Task<bool> ActualizarAsync(EstadoConciliacion estadoConciliacion);

        Task<bool> EliminarLogicoAsync(int id);
    }
}
