using GestionCuentasBancarias.Domain.DTOS.ConversionMoneda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IConversionMonedaService
    {
        Task<List<ResponseConversionMonedaDTO>> ObtenerConversiones();
        Task<ResponseConversionMonedaDTO> ObtenerConversionPorId(int id);
        Task<ResponseConversionMonedaDTO> ObtenerTasaVigente(int monedaOrigen, int monedaDestino, string fecha);
        Task CrearConversion(CreateConversionMonedaDTO dto);
        Task<bool> ActualizarConversion(int id, UpdateConversionMonedaDTO dto);
        Task<bool> EliminarConversion(int id);
        Task<bool> ReactivarConversion(int id);
        // Consulta Frankfurter API y guarda la tasa del día automáticamente
        Task<ResponseConversionMonedaDTO> ObtenerTasaDesdeApi(int monedaOrigenId, int monedaDestinoId);
    }
}
