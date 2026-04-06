using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class TasaInteres
    {
        public int TIN_Tasa_Interes { get; set; } 
        public int CUB_Cuenta { get; set; } 
        public int INF_Frecuencia { get; set; } 
        public decimal TIN_Porcentaje { get; set; } 
        public string TIN_Estado { get; set; } = "A";
        public DateTime TIN_Fecha_Creacion { get; set; } = DateTime.Now;
    }
}
