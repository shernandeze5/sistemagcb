using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Rol
{
    public class ActualizarRolDTO
    {
        public string ROL_NOMBRE { get; set; } = string.Empty;
        public string? ROL_DESCRIPCION { get; set; }

        public ActualizarRolDTO() { }

        public ActualizarRolDTO(string nombre, string? descripcion)
        {
            ROL_NOMBRE = nombre;
            ROL_DESCRIPCION = descripcion;
        }
    }
}
