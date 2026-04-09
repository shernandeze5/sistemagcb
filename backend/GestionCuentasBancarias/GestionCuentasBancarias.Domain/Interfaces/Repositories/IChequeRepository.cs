using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IChequeRepository
    {
        Task<IEnumerable<ChequeResponseDTO>> ObtenerChequesAsync();
        Task<IEnumerable<ChequeResponseDTO>> ObtenerChequesPorCuentaAsync(int cuentaId);
        Task<ChequeResponseDTO?> ObtenerChequePorIdAsync(int id);
        Task<bool> CrearChequeAsync(CreateChequeDTO dto);
        Task<bool> CambiarEstadoChequeAsync(int chequeId, UpdateDTOCheque dto);
    }
}