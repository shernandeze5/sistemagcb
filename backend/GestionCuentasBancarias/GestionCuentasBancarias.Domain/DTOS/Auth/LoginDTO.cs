using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Auth
{
    public class LoginDTO
    {
        public string USU_EMAIL { get; set; } = string.Empty;
        public string USU_PASSWORD { get; set; } = string.Empty;
    }
}
