using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TipoTelefono
{
    public class TipoTelefonoDTO
    {
        public int TIT_Tipo_Telefono { get; set; }
        public string TIT_Descripcion { get; set; } = string.Empty;
        public string TIT_Estado { get; set; } = string.Empty;
        public DateTime TIT_Fecha_Creacion { get; set; }
    }
}
