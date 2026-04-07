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
        public string AIN_Periodo { get; set; } = null!; // Ej: "2026-04"
        public DateTime AIN_Fecha_Aplicacion { get; set; }
    }
}
