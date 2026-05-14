using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Rol
{
    public class RolDTO
    {
        public int ROL_ROL { get; set; }
        public string ROL_NOMBRE { get; set; } = string.Empty;
        public string? ROL_DESCRIPCION { get; set; }
        public string ROL_ESTADO { get; set; } = string.Empty;
        public DateTime ROL_FECHA_CREACION { get; set; }

        public RolDTO() { }

        public RolDTO(
            int rol,
            string nombre,
            string? descripcion,
            string estado,
            DateTime fechaCreacion)
        {
            ROL_ROL = rol;
            ROL_NOMBRE = nombre;
            ROL_DESCRIPCION = descripcion;
            ROL_ESTADO = estado;
            ROL_FECHA_CREACION = fechaCreacion;
        }
    }
}
