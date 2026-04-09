using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.ReglaRecargo
{
    public class UpdateReglaRecargoDTO
    {
        public string RCA_Descripcion { get; set; } = null!;
        public string RCA_Origen { get; set; } = null!;
        public decimal RCA_Monto { get; set; }
        public string RCA_Frecuencia { get; set; } = null!;
        public int? RCA_Dia_Cobro { get; set; }
    }
}
