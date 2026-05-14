using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class Rol
    {
        public int ROL_ROL { get; set; }
        public string ROL_NOMBRE { get; set; } = string.Empty;
        public string? ROL_DESCRIPCION { get; set; }
        public string ROL_ESTADO { get; set; } = "A";
        public DateTime ROL_FECHA_CREACION { get; set; }
    }
}
