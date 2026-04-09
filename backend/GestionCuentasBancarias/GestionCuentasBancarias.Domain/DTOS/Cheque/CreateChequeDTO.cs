using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Cheque
{
    public class CreateChequeDTO
    {
        public int CUB_Cuenta { get; set; }
        public int PER_Persona { get; set; }
        public int CHQ_Chequera { get; set; }
        public int ESC_Estado_Cheque { get; set; }

        public string CHE_Numero_Cheque { get; set; }
        public string CHE_Monto_Letras { get; set; }
        public DateTime CHE_Fecha_Emision { get; set; }
        public DateTime? CHE_Fecha_Vencimiento { get; set; }
        public string CHE_Concepto { get; set; }
        public string MOV_Numero_Referencia { get; set; }
        public string MOV_Descripcion { get; set; }
        public decimal MOV_Monto { get; set; }

        public CreateChequeDTO()
        {
            CHE_Numero_Cheque = string.Empty;
            CHE_Monto_Letras = string.Empty;
            CHE_Concepto = string.Empty;
            MOV_Numero_Referencia = string.Empty;
            MOV_Descripcion = string.Empty;
        }

        public CreateChequeDTO(
            int cuB_Cuenta,
            int peR_Persona,
            int chQ_Chequera,
            int esC_Estado_Cheque,
            string chE_Numero_Cheque,
            string chE_Monto_Letras,
            DateTime chE_Fecha_Emision,
            DateTime? chE_Fecha_Vencimiento,
            string chE_Concepto,
            string moV_Numero_Referencia,
            string moV_Descripcion,
            decimal moV_Monto)
        {
            CUB_Cuenta = cuB_Cuenta;
            PER_Persona = peR_Persona;
            CHQ_Chequera = chQ_Chequera;
            ESC_Estado_Cheque = esC_Estado_Cheque;
            CHE_Numero_Cheque = chE_Numero_Cheque;
            CHE_Monto_Letras = chE_Monto_Letras;
            CHE_Fecha_Emision = chE_Fecha_Emision;
            CHE_Fecha_Vencimiento = chE_Fecha_Vencimiento;
            CHE_Concepto = chE_Concepto;
            MOV_Numero_Referencia = moV_Numero_Referencia;
            MOV_Descripcion = moV_Descripcion;
            MOV_Monto = moV_Monto;
        }
    }
}