using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.Conciliacion;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IEstadoConciliacionService
    {
        Task<IEnumerable<EstadoConciliacionDTO>> ObtenerTodosAsync();

        Task<EstadoConciliacionDTO?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(CrearEstadoConciliacionDTO dto);

        Task<bool> ActualizarAsync(int id, ActualizarEstadoConciliacionDTO dto);

        Task<bool> EliminarLogicoAsync(int id);
    }
}
