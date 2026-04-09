using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TasaInteres
{
    public class TasaInteresFrecuenciaDTO
    {
        public int TIN_Tasa_Interes { get; set; }
        public int CUB_Cuenta { get; set; }
        public decimal TIN_Porcentaje { get; set; }
        public string Frecuencia { get; set; } = null!;
    }
}
