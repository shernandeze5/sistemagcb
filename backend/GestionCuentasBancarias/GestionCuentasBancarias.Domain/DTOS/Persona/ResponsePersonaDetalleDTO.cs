using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Persona
{
    public class ResponsePersonaDetalleDTO
    {
        public int PER_Persona { get; set; }
        public int TIP_Tipo_Persona { get; set; }
        public string TipoPersonaDescripcion { get; set; } = string.Empty;
        public string PER_Primer_Nombre { get; set; } = string.Empty;
        public string PER_Segundo_Nombre { get; set; } = string.Empty;
        public string PER_Primer_Apellido { get; set; } = string.Empty;
        public string PER_Segundo_Apellido { get; set; } = string.Empty;
        public string PER_Razon_Social { get; set; } = string.Empty;
        public string PER_NIT { get; set; } = string.Empty;
        public string PER_DPI { get; set; } = string.Empty;
        public string PER_Estado { get; set; } = "A";
        public DateTime PER_Fecha_Creacion { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
    

        public List<ResponseTelefonoPersonaDTO> Telefonos { get; set; } = new();
        public List<ResponseDireccionPersonaDTO> Direcciones { get; set; } = new();
    }
}
