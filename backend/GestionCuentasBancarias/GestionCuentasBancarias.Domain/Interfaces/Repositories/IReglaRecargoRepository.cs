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
        Task<List<ResponseReglaRecargoDTO>> ObtenerReglas();
        Task<List<ResponseReglaRecargoDTO>> ObtenerReglasPorCuenta(int cuentaId);
        Task<ResponseReglaRecargoDTO> ObtenerReglaPorId(int id);
        Task CrearRegla(CreateReglaRecargoDTO dto);
        Task<bool> ActualizarRegla(int id, UpdateReglaRecargoDTO dto);
        Task<bool> EliminarRegla(int id);
        Task<bool> ReactivarRegla(int id);
    }
}
