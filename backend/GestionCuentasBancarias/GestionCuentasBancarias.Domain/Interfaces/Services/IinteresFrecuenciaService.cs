using GestionCuentasBancarias.Domain.DTOS.InteresFrecuencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IinteresFrecuenciaService
    {
        Task<List<ResponseInteresFrecuenciaDTO>> ObtenerFrecuencias();
        Task<ResponseInteresFrecuenciaDTO> ObtenerFrecuenciaPorId(int id);
        Task CrearFrecuencia(CreateInteresFrecuenciaDTO dto);
        Task<bool> ActualizarFrecuencia(int id, UpdateInteresFrecuenciaDTO dto);
        Task<bool> EliminarFrecuencia(int id);
        Task<bool> ReactivarFrecuencia(int id);
    }
}
