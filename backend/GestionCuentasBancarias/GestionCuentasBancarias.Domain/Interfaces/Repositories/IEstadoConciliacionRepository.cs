using GestionCuentasBancarias.Domain.DTOS.Conciliacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IEstadoConciliacionRepository
    {
        Task<IEnumerable<ResponseEstadoConciliacionDTO>> ObtenerEstadosConciliacion();
        Task<ResponseEstadoConciliacionDTO> ObtenerEstadoConciliacionPorId(int id);
        Task<int> CrearEstadoConciliacion(CreateEstadoConciliacionDTO dto);
        Task<bool> ActualizarEstadoConciliacion(int id, UpdateEstadoConciliacionDTO dto);
        Task<bool> EliminarEstadoConciliacion(int id);

        Task<bool> ReactivarEstadoConciliacion(int id);
    }
}
