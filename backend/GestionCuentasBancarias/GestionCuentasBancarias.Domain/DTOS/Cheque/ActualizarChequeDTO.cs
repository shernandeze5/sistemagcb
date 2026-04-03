using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Cheque
{
    public class ActualizarChequeDTO
    {
        public int MOV_Movimiento { get; set; }
        public string CHE_Numero_Cheque { get; set; } = string.Empty;
        public string CHE_Monto_Letras { get; set; } = string.Empty;
        public DateTime? CHE_Fecha_Emision { get; set; }
        public DateTime? CHE_Fecha_Cobro { get; set; }
        public DateTime? CHE_Fecha_Vencimiento { get; set; }
        public int ESC_Estado_Cheque { get; set; }
        public int? CHQ_Chequera { get; set; }
        public string CHE_Beneficiario { get; set; } = string.Empty;
        public string CHE_Concepto { get; set; } = string.Empty;
    }
}