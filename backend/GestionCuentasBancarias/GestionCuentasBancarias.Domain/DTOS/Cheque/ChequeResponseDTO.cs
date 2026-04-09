using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Cheque
{
    public class ChequeResponseDTO
    {
        public int CUB_Cuenta { get; set; }
        public int PER_Persona { get; set; }
        public int CHQ_Chequera { get; set; }
        public string CHE_Numero_Cheque { get; set; }
        public string CHE_Monto_Letras { get; set; }
        public DateTime CHE_Fecha_Emision { get; set; }
        public DateTime? CHE_Fecha_Vencimiento { get; set; }
        public string CHE_Concepto { get; set; }
        public string MOV_Numero_Referencia { get; set; }
        public string MOV_Descripcion { get; set; }
        public decimal MOV_Monto { get; set; }
    }
}