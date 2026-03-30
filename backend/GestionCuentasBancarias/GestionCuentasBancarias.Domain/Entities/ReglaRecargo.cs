using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class ReglaRecargo
    {
        public int RCA_Regla_Recargo { get; set; }
        public int CUB_Cuenta { get; set; }
        public string RCA_Descripcion { get; set; } = string.Empty;
        public string RCA_Origen { get; set; } = string.Empty;
        public decimal RCA_Monto { get; set; }
        public string RCA_Frecuencia { get; set; } = string.Empty;
        public int? RCA_Dia_Cobro { get; set; }
        public string RCA_Estado { get; set; } = "A";
        public DateTime RCA_Fecha_Creacion { get; set; } = DateTime.Now;
    }
}
