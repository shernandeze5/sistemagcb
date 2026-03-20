using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Conciliacion
{
    public class ResponseEstadoConciliacionDTO
    {
        public int ECO_Estado_Cuenta { get; set; }
        public string ECO_Descripcion { get; set; } = string.Empty;
        public string ECO_Estado { get; set; } = string.Empty;
        public DateTime ECO_Fecha_Creacion { get; set; }
    }
}
