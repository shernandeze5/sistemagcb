using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.NewFolder
{
    public class CrearTipoPersonaDTO
    {
        public string TIP_Descripcion { get; set; } = string.Empty;
        public string TIP_Estado { get; set; } = "A";
    }

}
