using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IMovimientoRepository
    {
        Task<List<ResponseMovimientoDTO>> ObtenerPorCuenta(int cuentaId);
        Task<ResponseMovimientoDTO?> ObtenerPorId(int id);
        Task<int> CrearMovimiento(CreateMovimientoDTO dto);
    }
}
