using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS
{
    public class MedioMovimientoDTO
    {
        public int MEM_Medio_Movimiento { get; set; }

        public string MEM_Descripcion { get; set; } = string.Empty;

        public int MEM_Estado { get; set; }

        public DateTime MEM_Fecha_Creacion { get; set; }
    }
}