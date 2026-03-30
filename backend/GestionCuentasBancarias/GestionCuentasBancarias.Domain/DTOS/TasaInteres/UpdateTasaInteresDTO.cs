using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TasaInteres
{
    public class UpdateTasaInteresDTO
    {
        public int INF_Frecuencia { get; set; }
        public decimal TIN_Porcentaje { get; set; }
    }
}
