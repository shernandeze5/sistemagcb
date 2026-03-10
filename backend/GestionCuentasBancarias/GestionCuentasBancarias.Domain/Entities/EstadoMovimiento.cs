using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class EstadoMovimiento
    {
        public int ESM_Estado_Movimiento { get; set; }

        public string ESM_Descripcion { get; set; } = string.Empty;

        public int ESM_Estado { get; set; }

        public DateTime ESM_Fecha_Creacion { get; set; }
    }
}