using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.TipoPersona
{
    public class CreateTipoPersonaDTO
    {
        public string TIP_Descripcion { get; set; } = string.Empty;
        public string TIP_Estado { get; set; } = "A";
    }

}
