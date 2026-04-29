using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GestionCuentasBancarias.Domain.DTOs.ReporteCheque
{
    public class ReporteChequeDTO
    {
        public int CHE_Cheque { get; set; }
        public int MOV_Movimiento { get; set; }
        public int CUB_Cuenta { get; set; }
        public int? CHQ_Chequera { get; set; }
        public int PER_Persona { get; set; }

        public string Beneficiario { get; set; } = string.Empty;
        public string CHE_Numero_Cheque { get; set; } = string.Empty;
        public string CHE_Monto_Letras { get; set; } = string.Empty;
        public DateTime? CHE_Fecha_Emision { get; set; }
        public DateTime? CHE_Fecha_Cobro { get; set; }
        public DateTime? CHE_Fecha_Vencimiento { get; set; }
        public string CHE_Concepto { get; set; } = string.Empty;

        public int ESC_Estado_Cheque { get; set; }
        public string EstadoCheque { get; set; } = string.Empty;

        public DateTime MOV_Fecha { get; set; }
        public string MOV_Numero_Referencia { get; set; } = string.Empty;
        public string MOV_Descripcion { get; set; } = string.Empty;
        public decimal MOV_Monto { get; set; }
        public decimal MOV_Monto_Origen { get; set; }
        public decimal MOV_Saldo { get; set; }
    }
}
