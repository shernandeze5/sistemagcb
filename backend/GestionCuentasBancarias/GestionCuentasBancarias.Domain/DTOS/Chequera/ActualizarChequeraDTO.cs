using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Chequera
{
    public class ActualizarChequeraDTO
    {
        public int CUB_Cuenta { get; set; }
        public string CHQ_Serie { get; set; } = string.Empty;
        public int CHQ_Numero_Desde { get; set; }
        public int CHQ_Numero_Hasta { get; set; }
        public int CHQ_Ultimo_Usado { get; set; }
        public string CHQ_Estado { get; set; } = string.Empty; // A / I
        public DateTime CHQ_Fecha_Recepcion { get; set; }
    }
}