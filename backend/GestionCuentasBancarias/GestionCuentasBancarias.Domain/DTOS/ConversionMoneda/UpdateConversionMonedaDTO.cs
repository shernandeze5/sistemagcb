using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.ConversionMoneda
{
    public class UpdateConversionMonedaDTO
    {
        public decimal COM_Tasa_Cambio { get; set; }
        public DateTime COM_Fecha_Vigencia { get; set; }
    }
}
