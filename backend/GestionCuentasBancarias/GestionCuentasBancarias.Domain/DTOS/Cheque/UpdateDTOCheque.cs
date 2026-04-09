using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.DTOS.Cheque
{
    public class UpdateDTOCheque
    {
        public int ESC_Estado_Cheque { get; set; }

        public UpdateDTOCheque() { }

        public UpdateDTOCheque(int esC_Estado_Cheque)
        {
            ESC_Estado_Cheque = esC_Estado_Cheque;
        }
    }
}
