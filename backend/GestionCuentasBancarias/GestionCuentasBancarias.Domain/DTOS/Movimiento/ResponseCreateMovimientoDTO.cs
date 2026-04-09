using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Movimiento
{
    public class ResponseCreateMovimientoDTO
    {
        public int MOV_Movimiento { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
