using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS
{
    public class CrearMovimientoDTO
    {
        public int CUB_Cuenta { get; set; }
        public int PER_Persona { get; set; }
        public int TIM_Tipo_Movimiento { get; set; }
        public int MEM_Medio_Movimiento { get; set; }
        public int ESM_Estado_Movimiento { get; set; }
        public string MOV_Numero_Referencia { get; set; } = string.Empty;
        public string MOV_Descripcion { get; set; } = string.Empty;
        public decimal MOV_Monto { get; set; }
    }
}