using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Persona
{
    public class CreateTelefonoPersonaExistenteDTO
    {
        public int TIT_Tipo_Telefono { get; set; }
        public string TEP_Numero { get; set; } = string.Empty;
        public string TEP_Principal { get; set; } = "N";
        public string TEP_Estado { get; set; } = "A";
    }
}
