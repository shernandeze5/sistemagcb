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
        Task<int> CrearConRecargo(CreateMovimientoDTO dto);

        Task<IEnumerable<ResponseMovimientoDTO>> ObtenerTodos();
        Task<ResponseMovimientoDTO?> ObtenerPorId(int id);
        Task<IEnumerable<ResponseMovimientoDTO>> ObtenerPorCuenta(int cuentaId);

        Task AnularConRecargo(int movimientoId);

        Task<bool> ExisteCuentaActiva(int cuentaId);
        Task<bool> ExistePersonaActiva(int personaId);
        Task<bool> ExisteTipoMovimientoActivo(int tipoId);
        Task<bool> ExisteMedioMovimientoActivo(int medioId);
        Task<bool> ExisteEstadoMovimientoActivo(int estadoId);
    }
}
