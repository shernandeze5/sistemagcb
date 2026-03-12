using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class TipoCuenta
    {
        public int TCU_Tipo_Cuenta { get; set; }
        public string TCU_Descripcion { get; set; } = string.Empty;
        public string TCU_Estado { get; set; } = string.Empty;
        public DateTime TCU_Fecha_Creacion { get; set; }
    }
}
