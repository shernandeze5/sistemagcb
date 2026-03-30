using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.ConversionMoneda
{
    public class ResponseConversionMonedaDTO
    {
        public int COM_Conversion_Moneda { get; set; }
        public int TMO_Tipo_Moneda { get; set; }
        public string TMO_Simbolo_Origen { get; set; } = string.Empty;
        public string TMO_Descripcion_Origen { get; set; } = string.Empty;
        public string TMO_Codigo_ISO_Origen { get; set; } = string.Empty;
        public int TMO_Tipo_Moneda_Destino { get; set; }
        public string TMO_Simbolo_Destino { get; set; } = string.Empty;
        public string TMO_Descripcion_Destino { get; set; } = string.Empty;
        public string TMO_Codigo_ISO_Destino { get; set; } = string.Empty;
        public decimal COM_Tasa_Cambio { get; set; }
        public DateTime COM_Fecha_Vigencia { get; set; }
        public string COM_Fuente { get; set; } = string.Empty;
        public string COM_Estado { get; set; } = string.Empty;
        public DateTime COM_Fecha_Creacion { get; set; }
    }
}
