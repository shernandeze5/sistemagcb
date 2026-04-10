namespace GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria
{
    public class GuardarConciliacionDTO
    {
        public int CUB_Cuenta { get; set; }
        public string CON_Periodo { get; set; } = string.Empty;
        public string ARC_Nombre_Archivo { get; set; } = string.Empty;
        public decimal CON_Saldo_Banco { get; set; }
        public decimal CON_Saldo_Libros { get; set; }
        public decimal CON_Diferencia { get; set; }
        public int ECO_Estado_Conciliacion { get; set; }
        public List<MovimientoTemporalImportDTO> Temporales { get; set; } = new();
        public List<CreateDetalleConciliacionDTO> Detalles { get; set; } = new();

        public GuardarConciliacionDTO()
        {
        }
    }
}