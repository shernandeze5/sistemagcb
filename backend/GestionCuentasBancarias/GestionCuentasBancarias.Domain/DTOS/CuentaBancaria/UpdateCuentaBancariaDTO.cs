using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.CuentaBancaria
{
    public class UpdateCuentaBancariaDTO
    {
        public string CUB_Primer_Nombre { get; set; } = string.Empty;
        public string CUB_Segundo_Nombre { get; set; } = string.Empty;
        public string CUB_Primer_Apellido { get; set; } = string.Empty;
        public string CUB_Segundo_Apellido { get; set; } = string.Empty;
        public int ESC_Estado_Cuenta { get; set; }
    }
}
