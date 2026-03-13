using GestionCuentasBancarias.Domain.DTOS.TipoMoneda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface ITipoMonedaService
    {
        Task<IEnumerable<ResponseTipoMonedaDTO>> ObtenerTiposMoneda();
        Task<ResponseTipoMonedaDTO> ObtenerTipoMonedaPorId(int id);
        Task<int> CrearTipoMoneda(CreateTipoMonedaDTO dto);
        Task<bool> ActualizarTipoMoneda(int id, UpdateTipoMonedaDTO dto);
        Task<bool> EliminarTipoMoneda(int id);
    }
}
