using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS
{
    public class ResponseMovimientoDTO
    {
        public int MOV_Movimiento { get; set; }
        public int CUB_Cuenta { get; set; }

        public decimal MOV_Monto { get; set; }
        public decimal MOV_Saldo { get; set; }

        public string? TIM_Descripcion { get; set; }
        public string? MEM_Descripcion { get; set; }
        public string? ESM_Descripcion { get; set; }

        public DateTime MOV_Fecha { get; set; }
    }
}