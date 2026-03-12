using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.DTOS.TipoCuenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface ITipoCuentaService
    {
        Task<IEnumerable<ResponseTipoCuentaDTO>> ObtenerTiposCuenta();
        Task<ResponseTipoCuentaDTO> ObtenerTipoCuentaPorId(int id);
        Task<int> CrearTipoCuenta(CreateTipoCuentaDTO dto);
        Task<bool> ActualizarTipoCuenta(int id, UpdateTipoCuentaDTO dto);
        Task<bool> EliminarTipoCuenta(int id);
    }
}
