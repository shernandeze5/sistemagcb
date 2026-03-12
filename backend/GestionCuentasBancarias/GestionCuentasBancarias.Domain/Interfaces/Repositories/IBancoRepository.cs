using GestionCuentasBancarias.Domain.DTOS.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IBancoRepository
    {
        Task<List<ResponseBancoDTO>> ObtenerBancos();
        Task<ResponseBancoDTO> ObtenerBancoPorId(int id);
        Task CrearBanco(CreateBancoDTO dto);
        Task<bool> ActualizarBanco(int id, UpdataBancoDTO dto);
        Task<bool> EliminarBanco(int id);
    }
}
