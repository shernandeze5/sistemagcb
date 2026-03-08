using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS
{
    public class TipoMovimientoDTO
    {
        public int TIM_Tipo_Movimiento { get; set; }
        public string TIM_Descripcion { get; set; } = string.Empty;
        public int TIM_Estado { get; set; }
        public DateTime TIM_Fecha_Creacion { get; set; }
    }
}