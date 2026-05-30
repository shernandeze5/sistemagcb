using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.ReporteMovimiento
{
    public class EstadoCuentaMovimientoDTO
    {
        public decimal SaldoInicial { get; set; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalDebitos { get; set; }
        public decimal TotalRecargos { get; set; }
        public decimal SaldoFinal { get; set; }
        public int TotalMovimientos { get; set; }

        public IEnumerable<ReporteMovimientoDTO> Movimientos { get; set; } =
            new List<ReporteMovimientoDTO>();
    }
}
