using GestionCuentasBancarias.Domain.DTOS.Persona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IPersonaService
    {
        Task<IEnumerable<ResponsePersonaDTO>> ObtenerTodosAsync();
        Task<ResponsePersonaDTO?> ObtenerPorIdAsync(int id);
        Task<ResponsePersonaDetalleDTO?> ObtenerDetallePorIdAsync(int id);
        Task<ResponseCreatePersonaDTO> CrearAsync(CreatePersonaDTO dto);
        Task<bool> ActualizarAsync(int id, UpdatePersonaDTO dto);
        Task<bool> EliminarLogicoAsync(int id);
        Task<bool> AgregarTelefonoAsync(int personaId, CreateTelefonoPersonaExistenteDTO dto);
        Task<bool> AgregarDireccionAsync(int personaId, CreateDireccionPersonaExistenteDTO dto);
        Task<bool> ActualizarTelefonoAsync(int telefonoId, UpdateTelefonoPersonaDTO dto);
        Task<bool> EliminarTelefonoLogicoAsync(int telefonoId);

        Task<bool> ActualizarDireccionAsync(int direccionId, UpdateDireccionPersonaDTO dto);
        Task<bool> EliminarDireccionLogicoAsync(int direccionId);
    }
}
