using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS
{
    public class UpdateMovimientoDTO
    {
        public string? MOV_Descripcion { get; set; }
        public string? MOV_Numero_Referencia { get; set; }
    }
}