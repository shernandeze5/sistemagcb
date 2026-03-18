using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.DetalleConciliacion
{
    public class EstadoDetalleConciliacionDTO
    {
        public int EDC_Estado_Detalle_Conciliacion { get; set; }
        public string EDC_Descripcion { get; set; } = string.Empty;
        public int EDC_Estado { get; set; }
        public DateTime EDC_Fecha_Creacion { get; set; }
    }
}