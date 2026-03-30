using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class InteresFrecuencia
    {
        public int INF_Interes_Frecuencia { get; set; }
        public string INF_Descripcion { get; set; } = string.Empty;
        public string INF_Estado { get; set; } = "A";
        public DateTime INF_Fecha_Creacion { get; set; } = DateTime.Now;
    }
}
