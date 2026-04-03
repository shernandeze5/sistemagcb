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
        Task<IEnumerable<ResponseChequeDTO>> ObtenerTodosAsync();
        Task<ResponseChequeDTO?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(CrearChequeDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarChequeDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}