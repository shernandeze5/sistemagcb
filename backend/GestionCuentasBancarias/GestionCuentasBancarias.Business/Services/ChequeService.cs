using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

namespace GestionCuentasBancarias.Business.Services
{
    public class ChequeService : IChequeService
    {
        private readonly IChequeRepository repository;

        public ChequeService(IChequeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ChequeResponseDTO>> ObtenerCheques()
        {
            return await repository.ObtenerChequesAsync();
        }

        public async Task<IEnumerable<ChequeResponseDTO>> ObtenerChequesPorCuenta(int cuentaId)
        {
            return await repository.ObtenerChequesPorCuentaAsync(cuentaId);
        }

        public async Task<ChequeResponseDTO?> ObtenerChequePorId(int id)
        {
            return await repository.ObtenerChequePorIdAsync(id);
        }

        public async Task CrearCheque(CreateChequeDTO dto)
        {
            string numeroReferencia = string.IsNullOrWhiteSpace(dto.MOV_Numero_Referencia)
                ? $"CH-{dto.CHE_Numero_Cheque}"
                : dto.MOV_Numero_Referencia.Trim();

            string descripcionMovimiento = string.IsNullOrWhiteSpace(dto.MOV_Descripcion)
                ? $"Emisión de cheque No. {dto.CHE_Numero_Cheque}"
                : dto.MOV_Descripcion.Trim();

            var cheque = new CreateChequeDTO(
                dto.CUB_Cuenta,
                dto.PER_Persona,
                dto.CHQ_Chequera,
                dto.ESC_Estado_Cheque,
                dto.CHE_Numero_Cheque,
                dto.CHE_Monto_Letras,
                dto.CHE_Fecha_Emision,
                dto.CHE_Fecha_Vencimiento,
                dto.CHE_Concepto,
                numeroReferencia,
                descripcionMovimiento,
                dto.MOV_Monto
            );

            if (cheque.CUB_Cuenta <= 0)
                throw new InvalidOperationException("La cuenta es obligatoria.");

            if (cheque.PER_Persona <= 0)
                throw new InvalidOperationException("La persona beneficiaria es obligatoria.");

            if (cheque.CHQ_Chequera <= 0)
                throw new InvalidOperationException("La chequera es obligatoria.");

            if (cheque.ESC_Estado_Cheque <= 0)
                throw new InvalidOperationException("El estado del cheque es obligatorio.");

            if (string.IsNullOrWhiteSpace(cheque.CHE_Numero_Cheque))
                throw new InvalidOperationException("El número de cheque es obligatorio.");

            if (cheque.MOV_Monto <= 0)
                throw new InvalidOperationException("El monto debe ser mayor a cero.");

            if (cheque.CHE_Fecha_Vencimiento.HasValue &&
                cheque.CHE_Fecha_Vencimiento.Value.Date < cheque.CHE_Fecha_Emision.Date)
                throw new InvalidOperationException("La fecha de vencimiento no puede ser menor a la fecha de emisión.");

            await repository.CrearChequeAsync(cheque);
        }

        public async Task<bool> CambiarEstadoCheque(int chequeId, UpdateDTOCheque dto)
        {
            var estadoDto = new UpdateDTOCheque(dto.ESC_Estado_Cheque);

            if (chequeId <= 0)
                throw new InvalidOperationException("El id del cheque es inválido.");

            if (estadoDto.ESC_Estado_Cheque <= 0)
                throw new InvalidOperationException("Debe enviar el id del estado del cheque.");

            return await repository.CambiarEstadoChequeAsync(chequeId, estadoDto);
        }
    }
}