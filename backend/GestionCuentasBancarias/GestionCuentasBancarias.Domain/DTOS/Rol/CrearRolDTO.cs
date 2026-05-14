using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Rol
{
    public class CrearRolDTO
    {
        public string ROL_NOMBRE { get; set; } = string.Empty;
        public string? ROL_DESCRIPCION { get; set; }

        public CrearRolDTO() { }

        public CrearRolDTO(string nombre, string? descripcion)
        {
            ROL_NOMBRE = nombre;
            ROL_DESCRIPCION = descripcion;
        }
    }
}
