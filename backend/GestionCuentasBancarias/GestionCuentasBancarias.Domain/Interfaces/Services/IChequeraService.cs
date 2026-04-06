using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS.Chequera;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IChequeraService
    {
        Task<IEnumerable<ChequeraDTO>> ObtenerTodosAsync();
        Task<ChequeraDTO?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(CrearChequeraDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarChequeraDTO dto);
        Task<bool> EliminarLogicoAsync(int id);
        Task<bool> Reactivar(int id);
    }
}