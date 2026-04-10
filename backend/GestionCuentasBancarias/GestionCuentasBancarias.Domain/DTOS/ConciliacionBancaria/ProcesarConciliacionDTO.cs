using Microsoft.AspNetCore.Http;

namespace GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria
{
    public class ProcesarConciliacionDTO
    {
        public int CUB_Cuenta { get; set; }
        public string CON_Periodo { get; set; } = string.Empty;
        public IFormFile? Archivo { get; set; }

        public ProcesarConciliacionDTO()
        {
        }
    }
}