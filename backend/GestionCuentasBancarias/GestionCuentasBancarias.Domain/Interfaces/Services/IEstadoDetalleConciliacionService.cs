using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.DetalleConciliacion;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IEstadoDetalleConciliacionService
    {
        Task<IEnumerable<EstadoDetalleConciliacionDTO>> ObtenerTodosAsync();

        Task<EstadoDetalleConciliacionDTO?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(CrearEstadoDetalleConciliacionDTO dto);

        Task<bool> ActualizarAsync(int id, ActualizarEstadoDetalleConciliacionDTO dto);

        Task<bool> EliminarLogicoAsync(int id);
    }
}
