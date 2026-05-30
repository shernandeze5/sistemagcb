using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.ReporteConciliacion
{
    public class ReporteConciliacionDTO
    {
        public int CON_Conciliacion { get; set; }
        public int CUB_Cuenta { get; set; }
        public string CUB_Numero_Cuenta { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public string CON_Periodo { get; set; } = string.Empty;
        public decimal CON_Saldo_Banco { get; set; }
        public decimal CON_Saldo_Libros { get; set; }
        public decimal CON_Diferencia { get; set; }
        public DateTime CON_Fecha_Conciliacion { get; set; }
        public int ECO_Estado_Conciliacion { get; set; }
        public string EstadoConciliacion { get; set; } = string.Empty;
        public int TotalConciliados { get; set; }
        public int TotalPendientesBanco { get; set; }
        public int TotalPendientesLibros { get; set; }
        public int TotalEnTransito { get; set; }
        public int TotalDiferencias { get; set; }
    }
}
