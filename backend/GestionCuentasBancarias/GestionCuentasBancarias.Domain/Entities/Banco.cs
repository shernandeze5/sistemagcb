using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Entities
{
    public class Banco
    {
        public int BAN_Banco { get; set; }
        public string BAN_Nombre { get; set; } = string.Empty;
        public string BAN_Codigo_Swift { get; set; } = string.Empty;
        public string BAN_Estado { get; set; } = string.Empty;
        public DateTime BAN_Fecha_Creacion { get; set; } = DateTime.Now;
    }
}
