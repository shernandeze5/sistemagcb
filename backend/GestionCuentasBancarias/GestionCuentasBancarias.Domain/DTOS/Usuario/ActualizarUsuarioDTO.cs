using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Usuario
{
    public class ActualizarUsuarioDTO
    {
        public int ROL_ROL { get; set; }

        public string USU_PRIMER_NOMBRE { get; set; } = string.Empty;
        public string? USU_SEGUNDO_NOMBRE { get; set; }
        public string USU_PRIMER_APELLIDO { get; set; } = string.Empty;
        public string? USU_SEGUNDO_APELLIDO { get; set; }

        public string USU_EMAIL { get; set; } = string.Empty;

        public ActualizarUsuarioDTO() { }
    }
}
