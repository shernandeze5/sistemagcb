using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.ReporteCuentaBancaria
{
    public class ReporteCuentaBancariaDTO
    {
        public int CUB_Cuenta { get; set; }
        public int BAN_Banco { get; set; }
        public string Banco { get; set; } = string.Empty;
        public string CUB_Numero_Cuenta { get; set; } = string.Empty;
        public string Titular { get; set; } = string.Empty;
        public int TCU_Tipo_Cuenta { get; set; }
        public string TipoCuenta { get; set; } = string.Empty;
        public int TMO_Tipo_Moneda { get; set; }
        public string TipoMoneda { get; set; } = string.Empty;
        public decimal CUB_Saldo_Inicial { get; set; }
        public decimal CUB_Saldo_Actual { get; set; }
        public int ESC_Estado_Cuenta { get; set; }
        public string EstadoCuenta { get; set; } = string.Empty;
        public string CUB_Estado { get; set; } = string.Empty;
        public DateTime CUB_Fecha_Creacion { get; set; }
    }
}
