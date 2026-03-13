using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TipoTelefono
{
    public class ActualizarTipoTelefonoDTO
    {
        public string TIT_Descripcion { get; set; } = string.Empty;
        public string TIT_Estado { get; set; } = "A";
    }
}
