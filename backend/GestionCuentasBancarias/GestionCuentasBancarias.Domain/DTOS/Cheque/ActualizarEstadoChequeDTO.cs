using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Cheque
{
    public class ActualizarEstadoChequeDTO
    {
        public string ESC_Descripcion { get; set; } = string.Empty;

        public int ESC_Estado { get; set; }
    }
}