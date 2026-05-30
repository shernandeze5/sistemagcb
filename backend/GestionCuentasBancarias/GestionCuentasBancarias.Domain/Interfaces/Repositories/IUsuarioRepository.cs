using GestionCuentasBancarias.Domain.DTOS.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<UsuarioDTO>> ObtenerTodos();
        Task<UsuarioDTO?> ObtenerPorId(int id);
        Task<UsuarioDTO?> ObtenerPorEmail(string email);
        Task<int> Crear(CrearUsuarioDTO dto, string passwordHash);
        Task<bool> Actualizar(int id, ActualizarUsuarioDTO dto);
        Task<bool> CambiarPassword(int id, string passwordHash);
        Task<bool> Desactivar(int id);
        Task<bool> Reactivar(int id);
        Task<bool> ActualizarUltimoAcceso(int id);
    }
}
