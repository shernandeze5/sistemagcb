using GestionCuentasBancarias.Domain.DTOS.ReglaRecargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IReglaRecargoService
    {
        Task<int> Crear(CreateReglaRecargoDTO dto);
        Task<IEnumerable<ResponseReglaRecargoDTO>> ObtenerPorCuenta(int cuentaId);
        Task Actualizar(int id, UpdateReglaRecargoDTO dto);
        Task Eliminar(int id);
    }
}
