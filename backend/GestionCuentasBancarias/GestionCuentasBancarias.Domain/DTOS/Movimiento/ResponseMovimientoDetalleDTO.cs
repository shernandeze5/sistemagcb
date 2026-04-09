using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Movimiento
{
    public class ResponseMovimientoDetalleDTO : ResponseMovimientoDTO
    {
        public string NumeroCuenta { get; set; } = string.Empty;
        public string NombrePersona { get; set; } = string.Empty;
        public string RecargoDescripcion { get; set; } = string.Empty;
        public string ConversionDescripcion { get; set; } = string.Empty;
    }
}
