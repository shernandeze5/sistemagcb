using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class TelefonoPersonaService : ITelefonoPersonaService
    {
        private readonly ITelefonoPersonaRepository repository;

        public TelefonoPersonaService(ITelefonoPersonaRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ResponseTelefonoPersonaDTO>> ObtenerPorPersonaAsync(int personaId)
        {
            if (personaId <= 0)
                throw new InvalidOperationException("El id de persona es inválido.");

            return await repository.ObtenerPorPersonaAsync(personaId);
        }

        public async Task<ResponseTelefonoPersonaDTO?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("El id de teléfono es inválido.");

            return await repository.ObtenerPorIdAsync(id);
        }

        public async Task<bool> CrearAsync(CreateTelefonoPersonaDTO dto)
        {
            if (dto.PER_Persona <= 0)
                throw new InvalidOperationException("La persona es requerida.");

            if (dto.TIT_Tipo_Telefono <= 0)
                throw new InvalidOperationException("El tipo de teléfono es requerido.");

            if (string.IsNullOrWhiteSpace(dto.TEP_Numero))
                throw new InvalidOperationException("El número es requerido.");

            // 🔥 NORMALIZAR
            dto.TEP_Principal = (dto.TEP_Principal ?? "N").ToUpper();

            // 🔥 VALIDAR STRING
            if (dto.TEP_Principal != "S" && dto.TEP_Principal != "N")
                throw new InvalidOperationException("TEP_Principal debe ser 'S' o 'N'.");

            return await repository.CrearAsync(dto);
        }

        public async Task<bool> ActualizarAsync(int id, UpdateTelefonoPersonaDTO dto)
        {
            if (id <= 0)
                throw new InvalidOperationException("El id de teléfono es inválido.");

            if (dto.TIT_Tipo_Telefono <= 0)
                throw new InvalidOperationException("El tipo de teléfono es requerido.");

            if (string.IsNullOrWhiteSpace(dto.TEP_Numero))
                throw new InvalidOperationException("El número es requerido.");

            // 🔥 NORMALIZAR
            dto.TEP_Principal = (dto.TEP_Principal ?? "N").ToUpper();
            dto.TEP_Estado = (dto.TEP_Estado ?? "A").ToUpper();

            // 🔥 VALIDAR
            if (dto.TEP_Principal != "S" && dto.TEP_Principal != "N")
                throw new InvalidOperationException("TEP_Principal debe ser 'S' o 'N'.");

            if (dto.TEP_Estado != "A" && dto.TEP_Estado != "I")
                throw new InvalidOperationException("TEP_Estado debe ser 'A' o 'I'.");

            return await repository.ActualizarAsync(id, dto);
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("El id de teléfono es inválido.");

            return await repository.EliminarLogicoAsync(id);
        }
    }
}