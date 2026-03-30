using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.ConversionMoneda
{
    public class ExchangeRateApiResponse
    {
        public string result { get; set; } = string.Empty;
        public string base_code { get; set; } = string.Empty;
        public string target_code { get; set; } = string.Empty;
        public decimal conversion_rate { get; set; }
    }
}
