using GestionCuentasBancarias.Domain.DTOS.Persona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface ITelefonoPersonaRepository
    {
        Task<IEnumerable<ResponseTelefonoPersonaDTO>> ObtenerPorPersonaAsync(int personaId);
        Task<ResponseTelefonoPersonaDTO?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(CreateTelefonoPersonaDTO dto);
        Task<bool> ActualizarAsync(int id, UpdateTelefonoPersonaDTO dto);
        Task<bool> EliminarLogicoAsync(int id);
    }
}
