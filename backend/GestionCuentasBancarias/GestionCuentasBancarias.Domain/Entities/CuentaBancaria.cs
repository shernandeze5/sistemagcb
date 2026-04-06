using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class CuentaBancaria
    {
        public int CUB_Cuenta { get; set; }
        public int BAN_Banco { get; set; }
        public string CUB_Numero_Cuenta { get; set; } = string.Empty;
        public string CUB_Primer_Nombre { get; set; } = string.Empty;
        public string CUB_Segundo_Nombre { get; set; } = string.Empty;
        public string CUB_Primer_Apellido { get; set; } = string.Empty;
        public string CUB_Segundo_Apellido { get; set; } = string.Empty;
        public int TCU_Tipo_Cuenta { get; set; }
        public int TMO_Tipo_Moneda { get; set; }
        public decimal CUB_Saldo_Inicial { get; set; }
        public decimal CUB_Saldo_Actual { get; set; }
        public int ESC_Estado_Cuenta { get; set; }
        public string CUB_Estado { get; set; } = "A";
        public DateTime CUB_Fecha_Creacion { get; set; } = DateTime.Now;
    }
}
