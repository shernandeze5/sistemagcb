using GestionCuentasBancarias.Domain.DTOS.EstadoCuenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IEstadoCuentaService
    {
        Task<IEnumerable<ResponseEstadoCuentaDTO>> ObtenerEstadosCuenta();
        Task<ResponseEstadoCuentaDTO> ObtenerEstadoCuentaPorId(int id);
        Task<int> CrearEstadoCuenta(CreateEstadoCuentaDTO dto);
        Task<bool> ActualizarEstadoCuenta(int id, UpdateEstadoCuentaDTO dto);
        Task<bool> EliminarEstadoCuenta(int id);
    }
}
