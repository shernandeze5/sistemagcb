using GestionCuentasBancarias.Domain.DTOS.ReglaRecargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IReglaRecargoRepository
    {
        Task<int> Crear(CreateReglaRecargoDTO dto);
        Task<IEnumerable<ResponseReglaRecargoDTO>> ObtenerPorCuenta(int cuentaId);
        Task Actualizar(int id, UpdateReglaRecargoDTO dto);
        Task Eliminar(int id);
        Task AplicarRecargoAutomatico(int cuentaId);
    }
}
