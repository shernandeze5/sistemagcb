using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.AplicacionInteres
{
    public class ResponseAplicacionInteresDTO
    {
        public int AIN_Aplicacion_Interes { get; set; }
        public int TIN_Tasa_Interes { get; set; }
        public int MOV_Movimiento { get; set; }
        public decimal AIN_Saldo_Base { get; set; }
        public decimal AIN_Monto_Calculado { get; set; }
        public string AIN_Periodo { get; set; } = null!;
        public DateTime AIN_Fecha_Aplicacion { get; set; }
    }
}
