using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

public class PersonaService : IPersonaService
{
    private readonly IPersonaRepository repository;

    public PersonaService(IPersonaRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<ResponsePersonaDTO>> ObtenerTodosAsync()
    {
        return await repository.ObtenerTodosAsync();
    }

    public async Task<ResponsePersonaDTO?> ObtenerPorIdAsync(int id)
    {
        return await repository.ObtenerPorIdAsync(id);
    }

    public async Task<ResponsePersonaDetalleDTO?> ObtenerDetallePorIdAsync(int id)
    {
        return await repository.ObtenerDetallePorIdAsync(id);
    }

    public async Task<ResponseCreatePersonaDTO> CrearAsync(CreatePersonaDTO dto)
    {
        if (dto.TIP_Tipo_Persona <= 0)
            throw new InvalidOperationException("El tipo de persona es requerido.");

        if (dto.Telefonos == null || dto.Telefonos.Count == 0)
            throw new InvalidOperationException("Debe ingresar al menos un teléfono.");

        if (dto.Direcciones == null || dto.Direcciones.Count == 0)
            throw new InvalidOperationException("Debe ingresar al menos una dirección.");

        if (dto.Telefonos.Count(x => x.TEP_Principal == "S") != 1)
            throw new InvalidOperationException("Debe existir un solo teléfono principal.");

        if (dto.Direcciones.Count(x => x.DIR_Principal == "S") != 1)
            throw new InvalidOperationException("Debe existir una sola dirección principal.");

        bool esJuridica = !string.IsNullOrWhiteSpace(dto.PER_Razon_Social);

        if (!esJuridica)
        {
            if (string.IsNullOrWhiteSpace(dto.PER_Primer_Nombre))
                throw new InvalidOperationException("El primer nombre es requerido.");

            if (string.IsNullOrWhiteSpace(dto.PER_Primer_Apellido))
                throw new InvalidOperationException("El primer apellido es requerido.");

            if (string.IsNullOrWhiteSpace(dto.PER_DPI))
                throw new InvalidOperationException("El DPI es requerido.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(dto.PER_NIT))
                throw new InvalidOperationException("El NIT es requerido para persona jurídica.");
        }

        foreach (var telefono in dto.Telefonos)
        {
            if (telefono.TIT_Tipo_Telefono <= 0)
                throw new InvalidOperationException("El tipo de teléfono es requerido.");

            if (string.IsNullOrWhiteSpace(telefono.TEP_Numero))
                throw new InvalidOperationException("Todos los teléfonos deben tener número.");
        }

        foreach (var direccion in dto.Direcciones)
        {
            if (direccion.TDI_Tipo_Direccion <= 0)
                throw new InvalidOperationException("El tipo de dirección es requerido.");
        }

        // 🔥 NORMALIZAR ESTADO (SI LO USAS EN CREATE)
        if (string.IsNullOrWhiteSpace(dto.PER_Estado))
            dto.PER_Estado = "A";

        dto.PER_Estado = dto.PER_Estado.ToUpper();

        if (dto.PER_Estado != "A" && dto.PER_Estado != "I")
            throw new InvalidOperationException("El estado debe ser 'A' o 'I'.");

        return await repository.CrearAsync(dto);
    }

    public async Task<bool> ActualizarAsync(int id, UpdatePersonaDTO dto)
    {
        if (id <= 0)
            throw new InvalidOperationException("El id de persona es inválido.");

        if (dto.TIP_Tipo_Persona == 2 && string.IsNullOrWhiteSpace(dto.PER_Razon_Social))
        {
            throw new InvalidOperationException("La razón social es requerida para empresas.");
        }

        if (string.IsNullOrWhiteSpace(dto.PER_Estado))
            throw new InvalidOperationException("El estado es requerido.");

        dto.PER_Estado = dto.PER_Estado.ToUpper();

        if (dto.PER_Estado != "A" && dto.PER_Estado != "I")
            throw new InvalidOperationException("El estado debe ser 'A' o 'I'.");

        bool esJuridica = !string.IsNullOrWhiteSpace(dto.PER_Razon_Social);

        if (!esJuridica)
        {
            if (string.IsNullOrWhiteSpace(dto.PER_Primer_Nombre))
                throw new InvalidOperationException("El primer nombre es requerido.");

            if (string.IsNullOrWhiteSpace(dto.PER_Primer_Apellido))
                throw new InvalidOperationException("El primer apellido es requerido.");

            if (string.IsNullOrWhiteSpace(dto.PER_DPI))
                throw new InvalidOperationException("El DPI es requerido.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(dto.PER_NIT))
                throw new InvalidOperationException("El NIT es requerido para persona jurídica.");
        }

        return await repository.ActualizarAsync(id, dto);
    }

    public async Task<bool> EliminarLogicoAsync(int id)
    {
        if (id <= 0)
            throw new InvalidOperationException("El id de persona es inválido.");

        return await repository.EliminarLogicoAsync(id);
    }
}