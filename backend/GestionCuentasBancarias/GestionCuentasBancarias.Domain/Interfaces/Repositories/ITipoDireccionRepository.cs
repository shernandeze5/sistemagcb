using GestionCuentasBancarias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface ITipoDireccionRepository
    {
        Task<IEnumerable<TipoDireccion>> ObtenerTodosAsync();
        Task<TipoDireccion?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(TipoDireccion tipoDireccion);
        Task<bool> ActualizarAsync(TipoDireccion tipoDireccion);
        Task<bool> EliminarAsync(int id);
    }
}
