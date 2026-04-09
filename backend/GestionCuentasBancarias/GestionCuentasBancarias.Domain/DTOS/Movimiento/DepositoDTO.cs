using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Movimiento
{
    namespace GestionCuentasBancarias.Domain.DTOS.Movimiento
    {
        public class CreateMovimientoDTO
        {
            public int CUB_Cuenta { get; set; }
            public int PER_Persona { get; set; }
            public int TIM_Tipo_Movimiento { get; set; }
            public int MEM_Medio_Movimiento { get; set; }
            public int? RCA_Regla_Recargo { get; set; }
            public int? COM_Conversion_Moneda { get; set; }
            public DateTime MOV_Fecha { get; set; } = DateTime.Now;
            public string MOV_Numero_Referencia { get; set; } = string.Empty;
            public string MOV_Descripcion { get; set; } = string.Empty;
            public decimal MOV_Monto { get; set; }
            public decimal? MOV_Monto_Origen { get; set; }
            public int ESM_Estado_Movimiento { get; set; }
        }
    }
}
