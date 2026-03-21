using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class EstadoCheque
    {
        public int ESC_Estado_Cheque { get; set; }

        public string ESC_Descripcion { get; set; } = string.Empty;

        public string ESC_Estado { get; set; }=string.Empty;

        public DateTime ESC_Fecha_Creacion { get; set; }
    }
}