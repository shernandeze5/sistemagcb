using GestionCuentasBancarias.Domain.DTOS.DetalleConciliacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IEstadoDetalleConciliacionRepository
    {
        Task<IEnumerable<ResponseEstadoDetalleConciliacionDTO>> ObtenerEstadosDetalleConciliacion();
        Task<ResponseEstadoDetalleConciliacionDTO> ObtenerEstadoDetalleConciliacionPorId(int id);
        Task<int> CrearEstadoDetalleConciliacion(CreateEstadoDetalleConciliacionDTO dto);
        Task<bool> ActualizarEstadoDetalleConciliacion(int id, UpdateEstadoDetalleConciliacionDTO dto);
        Task<bool> EliminarEstadoDetalleConciliacion(int id);

        Task<bool> ReactivarEstadoDetalleConciliacion(int id);
    }
}
