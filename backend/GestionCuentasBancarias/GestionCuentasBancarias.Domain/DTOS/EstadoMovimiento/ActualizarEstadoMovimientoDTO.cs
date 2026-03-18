using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS
{
    public class ActualizarEstadoMovimientoDTO
    {
        public string ESM_Descripcion { get; set; } = string.Empty;

        public string ESM_Estado { get; set; } = string.Empty;
    }
}