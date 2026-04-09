using GestionCuentasBancarias.Domain.DTOS.AplicacionInteres;
using GestionCuentasBancarias.Domain.DTOS.TasaInteres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IAplicacionInteresRepository
    {
        Task<int> AplicarInteres(CreateAplicacionInteresDTO dto);
        Task<IEnumerable<ResponseAplicacionInteresDTO>> ObtenerPorCuenta(int cuentaId);
        Task<IEnumerable<ResponseAplicacionInteresDTO>> ObtenerPorTasa(int tasaId);
        Task<IEnumerable<TasaInteresFrecuenciaDTO>> ObtenerTasasActivas();
        Task AplicarRecargoAutomatico(int cuentaId);
    }
}
