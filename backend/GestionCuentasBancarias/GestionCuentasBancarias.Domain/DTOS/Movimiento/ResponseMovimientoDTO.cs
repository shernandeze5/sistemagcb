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
        public string CUB_Numero_Cuenta { get; set; }

        public int? PER_Persona { get; set; }
        public string? PER_Nombre_Completo { get; set; }

        public int TIM_Tipo_Movimiento { get; set; }
        public string TIM_Descripcion { get; set; }

        public int MEM_Medio_Movimiento { get; set; }
        public string MEM_Descripcion { get; set; }

        public int ESM_Estado_Movimiento { get; set; }
        public string ESM_Descripcion { get; set; }

        public int? RCA_Regla_Recargo { get; set; }
        public decimal MOV_Monto_Origen { get; set; }
        public decimal MOV_Recargo { get; set; }
        public decimal MOV_Monto { get; set; }
        public decimal MOV_Saldo { get; set; }
        public DateTime MOV_Fecha { get; set; }
        public string? MOV_Numero_Referencia { get; set; }
        public string MOV_Descripcion { get; set; }
        public DateTime MOV_Fecha_Creacion { get; set; }
    }
}