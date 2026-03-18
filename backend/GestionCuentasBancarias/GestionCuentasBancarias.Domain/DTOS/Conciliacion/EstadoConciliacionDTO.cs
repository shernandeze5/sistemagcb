using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Conciliacion
{
    public class EstadoConciliacionDTO
    {
        public int ECO_Estado_Conciliacion { get; set; }
        public string ECO_Descripcion { get; set; } = string.Empty;
        public int ECO_Estado { get; set; }
        public DateTime ECO_Fecha_Creacion { get; set; }
    }
}