using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TasaInteres
{
    public class CreateTasaInteresDTO
    {
        public int CUB_Cuenta { get; set; }
        public int INF_Frecuencia { get; set; }
        public decimal TIN_Porcentaje { get; set; }
    }
}
