using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface ITipoMovimientoService
    {
        Task<IEnumerable<TipoMovimientoDTO>> ObtenerTodosAsync();
        Task<TipoMovimientoDTO?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(CrearTipoMovimientoDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarTipoMovimientoDTO dto);
        Task<bool> EliminarLogicoAsync(int id);
        Task<bool> Reactivar(int id);
    }
}
