using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IEstadoMovimientoService
    {
        Task<IEnumerable<EstadoMovimientoDTO>> ObtenerTodosAsync();

        Task<EstadoMovimientoDTO?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(CrearEstadoMovimientoDTO dto);

        Task<bool> ActualizarAsync(int id, ActualizarEstadoMovimientoDTO dto);

        Task<bool> EliminarLogicoAsync(int id);
    }
}
