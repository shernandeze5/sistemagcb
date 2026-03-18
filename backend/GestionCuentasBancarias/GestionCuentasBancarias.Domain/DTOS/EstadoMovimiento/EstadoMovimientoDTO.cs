using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS
{
    public class EstadoMovimientoDTO
    {
        public int ESM_Estado_Movimiento { get; set; }

        public string ESM_Descripcion { get; set; } = string.Empty;

        public string ESM_Estado { get; set; } = string.Empty;

        public DateTime ESM_Fecha_Creacion { get; set; }
    }
}