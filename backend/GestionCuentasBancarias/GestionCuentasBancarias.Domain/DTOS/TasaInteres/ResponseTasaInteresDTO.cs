using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TasaInteres
{
    public class ResponseTasaInteresDTO
    {
        public int TIN_Tasa_Interes { get; set; }
        public int CUB_Cuenta { get; set; }
        public string CUB_Numero_Cuenta { get; set; } = string.Empty;
        public string BAN_Nombre { get; set; } = string.Empty;
        public string TCU_Descripcion { get; set; } = string.Empty;
        public int INF_Frecuencia { get; set; }
        public string INF_Descripcion { get; set; } = string.Empty;
        public decimal TIN_Porcentaje { get; set; }
        public string TIN_Estado { get; set; } = string.Empty;
        public DateTime TIN_Fecha_Creacion { get; set; }
    }
}
