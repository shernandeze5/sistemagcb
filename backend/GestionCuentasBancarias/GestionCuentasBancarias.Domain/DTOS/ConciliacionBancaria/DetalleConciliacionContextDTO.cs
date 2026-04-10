using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria
{
    public class DetalleConciliacionContextDTO
    {
        public int DCO_Detalle_Conciliacion { get; set; }
        public int CON_Conciliacion { get; set; }
        public int CUB_Cuenta { get; set; }
        public int? MOV_Movimiento { get; set; }
        public int? MTE_Movimiento_Temporal { get; set; }

        public DateTime? MOV_Fecha { get; set; }
        public string? MOV_Numero_Referencia { get; set; }
        public string? MOV_Descripcion { get; set; }
        public decimal? MOV_Monto { get; set; }
        public string? TIM_Descripcion { get; set; }
        public string? MEM_Descripcion { get; set; }

        public DateTime? MTE_Fecha { get; set; }
        public string? MTE_Referencia { get; set; }
        public string? MTE_Descripcion { get; set; }
        public decimal? MTE_Debito { get; set; }
        public decimal? MTE_Credito { get; set; }
        public decimal? MTE_Saldo { get; set; }

        public int EDC_Estado_Detalle_Conciliacion { get; set; }
        public string EDC_Descripcion { get; set; } = string.Empty;

        public DetalleConciliacionContextDTO()
        {
        }
    }
}
