using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TasaInteres
{
    public class TasaInteresDTO
    {
        public int TIN_Tasa_Interes { get; set; }
        public int CUB_Cuenta { get; set; }
        public int INF_Frecuencia { get; set; }
        public decimal TIN_Porcentaje { get; set; } // decimal para que funcione con Dapper y Oracle NUMBER(8,4)
        public string TIN_Estado { get; set; } = null!;
    }
}
