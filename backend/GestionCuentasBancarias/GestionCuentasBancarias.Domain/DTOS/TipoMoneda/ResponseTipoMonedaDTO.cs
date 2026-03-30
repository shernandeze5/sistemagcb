using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TipoMoneda
{
    public class ResponseTipoMonedaDTO
    {
        public int TMO_Tipo_Moneda { get; set; }
        public string TMO_Descripcion { get; set; } = string.Empty;
        public string TMO_Simbolo { get; set; } = string.Empty;
        public string TMO_Estado { get; set; } = string.Empty;
        public string TMO_Codigo_ISO { get; set; } = string.Empty;
        public DateTime TMO_Fecha_Creacion { get; set; }
    }
}
