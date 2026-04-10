namespace GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria
{
    public class CreateDetalleConciliacionDTO
    {
        public int? MOV_Movimiento { get; set; }
        public int? TempKey { get; set; }
        public int EDC_Estado_Detalle_Conciliacion { get; set; }

        public CreateDetalleConciliacionDTO()
        {
        }
    }
}