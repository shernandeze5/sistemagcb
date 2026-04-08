using GestionCuentasBancarias.Domain.DTOS.AplicacionInteres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IAplicacionInteresService
    {
        Task<int> AplicarInteres(CreateAplicacionInteresDTO dto);
        Task<IEnumerable<ResponseAplicacionInteresDTO>> ObtenerPorCuenta(int cuentaId);
        Task<IEnumerable<ResponseAplicacionInteresDTO>> ObtenerPorTasa(int tasaId);

        Task AplicarInteresesAutomaticos();
    }
}
