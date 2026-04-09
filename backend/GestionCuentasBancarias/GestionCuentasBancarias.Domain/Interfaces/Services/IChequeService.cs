using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.Cheque;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IChequeService
    {
        Task<IEnumerable<ChequeResponseDTO>> ObtenerCheques();
        Task<IEnumerable<ChequeResponseDTO>> ObtenerChequesPorCuenta(int cuentaId);
        Task<ChequeResponseDTO?> ObtenerChequePorId(int id);
        Task CrearCheque(CreateChequeDTO dto);
        Task<bool> CambiarEstadoCheque(int chequeId, UpdateDTOCheque dto);
    }
}