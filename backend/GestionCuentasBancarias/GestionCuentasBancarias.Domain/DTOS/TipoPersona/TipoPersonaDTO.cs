using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TipoPersona
{
    public class TipoPersonaDTO
    {
        public int TIP_Tipo_Persona { get; set; }
        public string TIP_Descripcion { get; set; } = string.Empty;
        public string TIP_Estado { get; set; } = string.Empty;
        public DateTime TIP_Fecha_Creacion { get; set; }
    }

}
