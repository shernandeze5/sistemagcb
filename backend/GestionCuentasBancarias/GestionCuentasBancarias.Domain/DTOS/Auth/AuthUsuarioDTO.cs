using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Auth
{
    public class AuthUsuarioDTO
    {
        public int USU_USUARIO { get; set; }
        public int ROL_ROL { get; set; }
        public string ROL_NOMBRE { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;
        public string USU_EMAIL { get; set; } = string.Empty;
        public string USU_PASSWORD { get; set; } = string.Empty;
        public string USU_ESTADO { get; set; } = string.Empty;
    }
}
