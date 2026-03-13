using GestionCuentasBancarias.Domain.DTOS.NewFolder;
using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface ITipoPersonaService
    {
        Task<IEnumerable<TipoPersonaDTO>> ObtenerTodosAsync();
        Task<TipoPersonaDTO?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(CrearTipoPersonaDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarTipoPersonaDTO dto);
        Task<bool> EliminarLogicoAsync(int id);
    }

}
