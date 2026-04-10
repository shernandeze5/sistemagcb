namespace GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria
{
    public class MovimientoSistemaConciliacionDTO
    {
        public int MOV_Movimiento { get; set; }
        public int CUB_Cuenta { get; set; }
        public DateTime MOV_Fecha { get; set; }
        public string? MOV_Numero_Referencia { get; set; }
        public string MOV_Descripcion { get; set; }
        public decimal MOV_Monto { get; set; }
        public string TIM_Descripcion { get; set; } = string.Empty;
        public string MEM_Descripcion { get; set; }
        public string ESM_Descripcion { get; set; }

        public MovimientoSistemaConciliacionDTO() 
        {
        }
    }
}