using GestionCuentasBancarias.Domain.DTOS.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDTO>> ObtenerTodos();
        Task<UsuarioDTO?> ObtenerPorId(int id);
        Task<int> Crear(CrearUsuarioDTO dto);
        Task<bool> Actualizar(int id, ActualizarUsuarioDTO dto);
        Task<bool> CambiarPassword(int id, CambiarPasswordDTO dto);
        Task<bool> Desactivar(int id);
        Task<bool> Reactivar(int id);
        Task<bool> ActualizarUltimoAcceso(int id);
    }
}
