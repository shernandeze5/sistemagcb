using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Usuario
{
    public class CambiarPasswordDTO
    {
        public string NuevaPassword { get; set; } = string.Empty;

        public CambiarPasswordDTO() { }

        public CambiarPasswordDTO(string nuevaPassword)
        {
            NuevaPassword = nuevaPassword;
        }
    }
}
