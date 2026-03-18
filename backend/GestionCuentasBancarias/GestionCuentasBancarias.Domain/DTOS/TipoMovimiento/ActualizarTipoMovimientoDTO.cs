using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS
{
    public class ActualizarTipoMovimientoDTO
    {
        public string TIM_Descripcion { get; set; } = string.Empty;
        public string TIM_Estado { get; set; } = string.Empty;
            
    }
}
