using GestionCuentasBancarias.Domain.DTOS.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<AuthUsuarioDTO?> ObtenerUsuarioPorEmail(string email);
        Task<bool> ActualizarUltimoAcceso(int usuarioId);
    }
}
