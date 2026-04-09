using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class TelefonoPersona
    {
        public int TEL_Telefono { get; set; }
        public int PER_Persona { get; set; }
        public int TIT_Tipo_Telefono { get; set; }
        public string TEP_Numero { get; set; } = string.Empty;
        public int TEP_Principal { get; set; }
        public int TEP_Estado { get; set; }
        public DateTime TEP_Fecha_Creacion { get; set; }
    }
}
