using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IMovimientoService
    {
        Task<IEnumerable<MovimientoDTO>> ObtenerTodosAsync();

        Task<MovimientoDTO?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(CrearMovimientoDTO dto);

        Task<bool> ActualizarAsync(int id, ActualizarMovimientoDTO dto);

        Task<bool> EliminarLogicoAsync(int id);
    }
}