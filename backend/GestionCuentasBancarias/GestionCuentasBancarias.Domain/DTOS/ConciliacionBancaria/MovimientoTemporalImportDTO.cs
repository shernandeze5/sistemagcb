namespace GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria
{
    public class MovimientoTemporalImportDTO
    {
        public int TempKey { get; set; }
        public DateTime MTE_Fecha { get; set; }
        public string MTE_Descripcion { get; set; } = string.Empty;
        public string? MTE_Referencia { get; set; }
        public decimal MTE_Debito { get; set; }
        public decimal MTE_Credito { get; set; }
        public decimal MTE_Saldo { get; set; }

        public MovimientoTemporalImportDTO()
        {
        }
    }
}