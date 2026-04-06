using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class ConversionMoneda
    {
        public int COM_Conversion_Moneda { get; set; }
        public int TMO_Tipo_Moneda { get; set; }
        public int TMO_Tipo_Moneda_Destino { get; set; }
        public decimal COM_Tasa_Cambio { get; set; }
        public DateTime COM_Fecha_Vigencia { get; set; }
        public string COM_Fuente { get; set; } = "M";
        public string COM_Estado { get; set; } = "A";
        public DateTime COM_Fecha_Creacion { get; set; } = DateTime.Now;
    }
}
