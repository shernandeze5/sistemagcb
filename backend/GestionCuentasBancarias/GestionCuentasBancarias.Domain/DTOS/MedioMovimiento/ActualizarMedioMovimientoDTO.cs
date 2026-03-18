using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.MedioMovimiento
{
    public class ActualizarMedioMovimientoDTO
    {
        public string MEM_Descripcion { get; set; } = string.Empty;

        public string MEM_Estado { get; set; } = string.Empty;
    }
}