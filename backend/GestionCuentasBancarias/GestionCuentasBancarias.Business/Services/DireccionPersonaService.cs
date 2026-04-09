using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class DireccionPersonaService : IDireccionPersonaService
    {
        private readonly IDireccionPersonaRepository repository;

        public DireccionPersonaService(IDireccionPersonaRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ResponseDireccionPersonaDTO>> ObtenerPorPersonaAsync(int personaId)
        {
            if (personaId <= 0)
                throw new InvalidOperationException("El id de persona es inválido.");

            return await repository.ObtenerPorPersonaAsync(personaId);
        }

        public async Task<ResponseDireccionPersonaDTO?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("El id de dirección es inválido.");

            return await repository.ObtenerPorIdAsync(id);
        }

        public async Task<bool> CrearAsync(CreateDireccionPersonaDTO dto)
        {
            if (dto.PER_Persona <= 0)
                throw new InvalidOperationException("La persona es requerida.");

            if (dto.TDI_Tipo_Direccion <= 0)
                throw new InvalidOperationException("El tipo de dirección es requerido.");

            dto.DIR_Principal = (dto.DIR_Principal ?? "N").ToUpper();

            if (dto.DIR_Principal != "S" && dto.DIR_Principal != "N")
                throw new InvalidOperationException("DIR_Principal debe ser 'S' o 'N'.");

            return await repository.CrearAsync(dto);
        }

        public async Task<bool> ActualizarAsync(int id, UpdateDireccionPersonaDTO dto)
        {
            if (id <= 0)
                throw new InvalidOperationException("El id de dirección es inválido.");

            if (dto.TDI_Tipo_Direccion <= 0)
                throw new InvalidOperationException("El tipo de dirección es requerido.");

         
            dto.DIR_Principal = (dto.DIR_Principal ?? "N").ToUpper();
            dto.DIR_Estado = (dto.DIR_Estado ?? "A").ToUpper();

         
            if (dto.DIR_Principal != "S" && dto.DIR_Principal != "N")
                throw new InvalidOperationException("DIR_Principal debe ser 'S' o 'N'.");

            if (dto.DIR_Estado != "A" && dto.DIR_Estado != "I")
                throw new InvalidOperationException("DIR_Estado debe ser 'A' o 'I'.");

            return await repository.ActualizarAsync(id, dto);
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("El id de dirección es inválido.");

            return await repository.EliminarLogicoAsync(id);
        }
    }
}