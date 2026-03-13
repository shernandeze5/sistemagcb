using GestionCuentasBancarias.Domain.DTOS.TipoDireccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface ITipoDireccionService
    {
        Task<IEnumerable<TipoDireccionDTO>> ObtenerTodosAsync();
        Task<bool> CrearAsync(CrearTipoDireccionDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarTipoDireccionDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
