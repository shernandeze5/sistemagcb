using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class MovimientoService : IMovimientoService
    {
        private readonly IMovimientoRepository repository;

        public MovimientoService(IMovimientoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> Crear(CreateMovimientoDTO dto)
        {
            ValidarDto(dto);
            await ValidarCatalogos(dto);

            return await repository.CrearConRecargo(dto);
        }

        public async Task<IEnumerable<ResponseMovimientoDTO>> ObtenerTodos()
        {
            return await repository.ObtenerTodos();
        }

        public async Task<ResponseMovimientoDTO?> ObtenerPorId(int id)
        {
            return await repository.ObtenerPorId(id);
        }

        public async Task<IEnumerable<ResponseMovimientoDTO>> ObtenerPorCuenta(int cuentaId)
        {
            if (cuentaId <= 0)
                throw new Exception("La cuenta es obligatoria.");

            return await repository.ObtenerPorCuenta(cuentaId);
        }

        public async Task Anular(int id)
        {
            if (id <= 0)
                throw new Exception("El id del movimiento es inválido.");

            await repository.AnularConRecargo(id);
        }

        private void ValidarDto(CreateMovimientoDTO dto)
        {
            if (dto.CUB_Cuenta <= 0)
                throw new Exception("La cuenta es obligatoria.");

            if (dto.TIM_Tipo_Movimiento <= 0)
                throw new Exception("El tipo de movimiento es obligatorio.");

            if (dto.MEM_Medio_Movimiento <= 0)
                throw new Exception("El medio de movimiento es obligatorio.");

            if (dto.ESM_Estado_Movimiento <= 0)
                throw new Exception("El estado de movimiento es obligatorio.");

            if (dto.MOV_Monto_Origen <= 0)
                throw new Exception("El monto debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(dto.MOV_Descripcion))
                throw new Exception("La descripción es obligatoria.");
        }

        private async Task ValidarCatalogos(CreateMovimientoDTO dto)
        {
            var cuentaExiste = await repository.ExisteCuentaActiva(dto.CUB_Cuenta);
            if (!cuentaExiste)
                throw new Exception("La cuenta bancaria no existe o está inactiva.");

            if (dto.PER_Persona.HasValue)
            {
                var personaExiste = await repository.ExistePersonaActiva(dto.PER_Persona.Value);
                if (!personaExiste)
                    throw new Exception("La persona no existe o está inactiva.");
            }

            var tipoExiste = await repository.ExisteTipoMovimientoActivo(dto.TIM_Tipo_Movimiento);
            if (!tipoExiste)
                throw new Exception("El tipo de movimiento no existe o está inactivo.");

            var medioExiste = await repository.ExisteMedioMovimientoActivo(dto.MEM_Medio_Movimiento);
            if (!medioExiste)
                throw new Exception("El medio de movimiento no existe o está inactivo.");

            var estadoExiste = await repository.ExisteEstadoMovimientoActivo(dto.ESM_Estado_Movimiento);
            if (!estadoExiste)
                throw new Exception("El estado de movimiento no existe o está inactivo.");
        }
    }
}