using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class Conciliacion
    {
        public int CON_Conciliacion { get; set; }
        public int CUB_Cuenta { get; set; }
        public string CON_Periodo { get; set; } = string.Empty;
        public decimal CON_Saldo_Banco { get; set; }
        public decimal CON_Saldo_Libros { get; set; }
        public decimal CON_Diferencia { get; set; }
        public DateTime CON_Fecha_Conciliacion { get; set; } = DateTime.Now;
        public int ECO_Estado_Conciliacion { get; set; }
    }
}
