using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Persona
{
    public class CreateDireccionPersonaExistenteDTO
    {
        public int TDI_Tipo_Direccion { get; set; }
        public string DIR_Departamento { get; set; } = string.Empty;
        public string DIR_Municipio { get; set; } = string.Empty;
        public string DIR_Colonia { get; set; } = string.Empty;
        public string DIR_Zona { get; set; } = string.Empty;
        public string DIR_Numero_Casa { get; set; } = string.Empty;
        public string DIR_Detalle { get; set; } = string.Empty;
        public string DIR_Principal { get; set; } = "N";
    }
}
