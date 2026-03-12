using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Banco
{
    public class CreateBancoDTO
    {
        public string BAN_Nombre { get; set; } = string.Empty;
        public string BAN_Codigo_Swift { get; set; } = string.Empty;
    }
}
