using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.ReglaRecargo
{
    public class CreateReglaRecargoDTO
    {
        public int CUB_Cuenta { get; set; }
        public string RCA_Descripcion { get; set; } = null!;
        public string RCA_Origen { get; set; } = null!; // C,Q,T,S
        public decimal RCA_Monto { get; set; }
        public string RCA_Frecuencia { get; set; } = null!; // M,U,O
        public int? RCA_Dia_Cobro { get; set; }
    }
}
