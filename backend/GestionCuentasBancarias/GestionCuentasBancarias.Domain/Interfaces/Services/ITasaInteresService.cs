using GestionCuentasBancarias.Domain.DTOS.TasaInteres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface ITasaInteresService
    {
        Task<List<ResponseTasaInteresDTO>> ObtenerTasas();
        Task<ResponseTasaInteresDTO> ObtenerTasaPorId(int id);
        Task CrearTasa(CreateTasaInteresDTO dto);
        Task<bool> ActualizarTasa(int id, UpdateTasaInteresDTO dto);
        Task<bool> EliminarTasa(int id);
        Task<bool> ReactivarTasa(int id);
    }
}
