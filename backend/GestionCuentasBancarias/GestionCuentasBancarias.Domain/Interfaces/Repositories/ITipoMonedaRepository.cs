using GestionCuentasBancarias.Domain.DTOS.TipoCuenta;
using GestionCuentasBancarias.Domain.DTOS.TipoMoneda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface ITipoMonedaRepository
    {
        Task<IEnumerable<ResponseTipoMonedaDTO>> ObtenerTiposMoneda();
        Task<ResponseTipoMonedaDTO> ObtenerTipoMonedaPorId(int id);
        Task<int> CrearTipoMoneda(CreateTipoMonedaDTO dto);
        Task<bool> ActualizarTipoMoneda(int id, UpdateTipoMonedaDTO dto);
        Task<bool> EliminarTipoMoneda(int id);
        Task<bool> ReactivarTipoMoneda(int id);
    }
}
