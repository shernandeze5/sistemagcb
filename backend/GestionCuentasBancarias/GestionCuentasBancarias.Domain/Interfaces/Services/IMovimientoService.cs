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
        Task<int> Crear(CreateMovimientoDTO dto);
        Task<IEnumerable<ResponseMovimientoDTO>> ObtenerTodos();
        Task<ResponseMovimientoDTO?> ObtenerPorId(int id);
        Task<IEnumerable<ResponseMovimientoDTO>> ObtenerPorCuenta(int cuentaId);
        Task Anular(int id);
    }
}