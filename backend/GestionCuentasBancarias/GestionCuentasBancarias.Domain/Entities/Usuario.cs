using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class Usuario
    {
        public int USU_USUARIO { get; set; }
        public int ROL_ROL { get; set; }

        public string USU_PRIMER_NOMBRE { get; set; } = string.Empty;
        public string? USU_SEGUNDO_NOMBRE { get; set; }
        public string USU_PRIMER_APELLIDO { get; set; } = string.Empty;
        public string? USU_SEGUNDO_APELLIDO { get; set; }

        public string USU_EMAIL { get; set; } = string.Empty;
        public string USU_PASSWORD { get; set; } = string.Empty;
        public string USU_ESTADO { get; set; } = "A";

        public DateTime USU_FECHA_CREACION { get; set; }
        public DateTime? USU_ULTIMO_ACCESO { get; set; }
    }
}
