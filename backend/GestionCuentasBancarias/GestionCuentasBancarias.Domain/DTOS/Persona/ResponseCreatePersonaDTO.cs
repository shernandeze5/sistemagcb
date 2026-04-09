using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Persona
{
    public class ResponseCreatePersonaDTO
    {
        public int PER_Persona { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
