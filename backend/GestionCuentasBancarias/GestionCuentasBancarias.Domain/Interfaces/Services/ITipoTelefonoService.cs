using GestionCuentasBancarias.Domain.DTOS.TipoTelefono;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface ITipoTelefonoService
    {
        Task<IEnumerable<TipoTelefonoDTO>> ObtenerTodosAsync();
        Task<bool> CrearAsync(CrearTipoTelefonoDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarTipoTelefonoDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
