using GestionCuentasBancarias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface ITipoTelefonoRepository
    {
        Task<IEnumerable<TipoTelefono>> ObtenerTodosAsync();
        Task<TipoTelefono?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(TipoTelefono tipoTelefono);
        Task<bool> ActualizarAsync(TipoTelefono tipoTelefono);
        Task<bool> EliminarAsync(int id);
    }
}
