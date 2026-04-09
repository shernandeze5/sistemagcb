using GestionCuentasBancarias.Domain.DTOS.Persona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IDireccionPersonaRepository
    {
        Task<IEnumerable<ResponseDireccionPersonaDTO>> ObtenerPorPersonaAsync(int personaId);
        Task<ResponseDireccionPersonaDTO?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(CreateDireccionPersonaDTO dto);
        Task<bool> ActualizarAsync(int id, UpdateDireccionPersonaDTO dto);
        Task<bool> EliminarLogicoAsync(int id);
    }
}
