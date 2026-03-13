using GestionCuentasBancarias.Domain.DTOS.NewFolder;
using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface ITipoPersonaRepository
    {
        Task<IEnumerable<TipoPersona>> ObtenerTodosAsync();
        Task<TipoPersona?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(TipoPersona entidad);
        Task<bool> ActualizarAsync(TipoPersona entidad);
        Task<bool> EliminarLogicoAsync(int id);
    }

}
