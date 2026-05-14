using GestionCuentasBancarias.Domain.DTOS.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Login(LoginDTO dto);
    }
}
