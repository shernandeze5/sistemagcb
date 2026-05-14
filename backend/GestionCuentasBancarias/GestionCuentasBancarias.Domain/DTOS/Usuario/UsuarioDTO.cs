using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Usuario
{
    public class UsuarioDTO
    {
        public int USU_USUARIO { get; set; }
        public int ROL_ROL { get; set; }
        public string ROL_NOMBRE { get; set; } = string.Empty;

        public string USU_PRIMER_NOMBRE { get; set; } = string.Empty;
        public string? USU_SEGUNDO_NOMBRE { get; set; }
        public string USU_PRIMER_APELLIDO { get; set; } = string.Empty;
        public string? USU_SEGUNDO_APELLIDO { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;

        public string USU_EMAIL { get; set; } = string.Empty;
        public string USU_ESTADO { get; set; } = string.Empty;

        public DateTime USU_FECHA_CREACION { get; set; }
        public DateTime? USU_ULTIMO_ACCESO { get; set; }

        public UsuarioDTO() { }

        public UsuarioDTO(
            int usuario,
            int rol,
            string rolNombre,
            string primerNombre,
            string? segundoNombre,
            string primerApellido,
            string? segundoApellido,
            string email,
            string estado,
            DateTime fechaCreacion,
            DateTime? ultimoAcceso)
        {
            USU_USUARIO = usuario;
            ROL_ROL = rol;
            ROL_NOMBRE = rolNombre;
            USU_PRIMER_NOMBRE = primerNombre;
            USU_SEGUNDO_NOMBRE = segundoNombre;
            USU_PRIMER_APELLIDO = primerApellido;
            USU_SEGUNDO_APELLIDO = segundoApellido;
            USU_EMAIL = email;
            USU_ESTADO = estado;
            USU_FECHA_CREACION = fechaCreacion;
            USU_ULTIMO_ACCESO = ultimoAcceso;

            NombreCompleto = string.Join(" ", new[]
            {
                primerNombre,
                segundoNombre,
                primerApellido,
                segundoApellido
            }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
