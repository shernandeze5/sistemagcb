using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.Entities;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IEstadoDetalleConciliacionRepository
    {
        Task<IEnumerable<EstadoDetalleConciliacion>> ObtenerTodosAsync();

        Task<EstadoDetalleConciliacion?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(EstadoDetalleConciliacion estadoDetalleConciliacion);

        Task<bool> ActualizarAsync(EstadoDetalleConciliacion estadoDetalleConciliacion);

        Task<bool> EliminarLogicoAsync(int id);
    }
}
