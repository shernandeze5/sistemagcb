using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Banco
{
    public class UpdataBancoDTO
    {
        public string BAN_Nombre { get; set; }
        public string BAN_Codigo_Swift { get; set; }
    }
}
