using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.DTOS.Cheque;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IEstadoChequeService
    {
        Task<IEnumerable<EstadoChequeDTO>> ObtenerTodosAsync();

        Task<EstadoChequeDTO?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(CrearEstadoChequeDTO dto);

        Task<bool> ActualizarAsync(int id, ActualizarEstadoChequeDTO dto);

        Task<bool> EliminarLogicoAsync(int id);
    }
}
