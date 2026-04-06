using GestionCuentasBancarias.Domain.DTOS.ConversionMoneda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IConversionMonedaRepository
    {
        Task<List<ResponseConversionMonedaDTO>> ObtenerConversiones();
        Task<ResponseConversionMonedaDTO> ObtenerConversionPorId(int id);
        Task<ResponseConversionMonedaDTO> ObtenerTasaVigente(int monedaOrigen, int monedaDestino, string fecha);
        Task CrearConversion(CreateConversionMonedaDTO dto);
        Task<string> ObtenerCodigoISO(int tipoMonedaId);
        Task<bool> ActualizarConversion(int id, UpdateConversionMonedaDTO dto);
        Task<bool> EliminarConversion(int id);
        Task<bool> ReactivarConversion(int id);
    }
}
