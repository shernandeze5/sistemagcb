using GestionCuentasBancarias.Domain.DTOS.Cheque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IEstadoChequeService
    {
        Task<IEnumerable<ResponseEstadoChequeDTO>> ObtenerEstadosCheque();
        Task<ResponseEstadoChequeDTO> ObtenerEstadoChequePorId(int id);
        Task<int> CrearEstadoCheque(CreateEstadoChequeDTO dto);
        Task<bool> ActualizarEstadoCheque(int id, UpdateChequeDTO dto);
        Task<bool> EliminarEstadoCheque(int id);
        Task<bool> ReactivarEstadoCheque(int id);
    }
}
