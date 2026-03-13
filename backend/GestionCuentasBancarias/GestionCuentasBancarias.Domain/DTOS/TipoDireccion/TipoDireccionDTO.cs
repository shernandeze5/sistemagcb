using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TipoDireccion
{
    public class TipoDireccionDTO
    {
        public int TDI_Tipo_Direccion { get; set; }
        public string TDI_Descripcion { get; set; } = string.Empty;
        public string TDI_Estado { get; set; } = string.Empty;
        public DateTime TDI_Fecha_Creacion { get; set; }
    }
}
