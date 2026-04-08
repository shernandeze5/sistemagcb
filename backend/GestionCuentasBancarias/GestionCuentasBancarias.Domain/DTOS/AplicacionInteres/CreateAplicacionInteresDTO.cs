using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.AplicacionInteres
{
    public class CreateAplicacionInteresDTO
    {
        public int TIN_Tasa_Interes { get; set; }
        public string Periodo { get; set; } = null!;
    }
}
