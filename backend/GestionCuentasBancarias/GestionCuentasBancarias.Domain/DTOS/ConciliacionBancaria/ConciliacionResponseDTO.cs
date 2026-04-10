namespace GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria
{
    public class ConciliacionResponseDTO
    {
        public int CON_Conciliacion { get; set; }
        public int CUB_Cuenta { get; set; }
        public string CON_Periodo { get; set; } = string.Empty;
        public decimal CON_Saldo_Banco { get; set; }
        public decimal CON_Saldo_Libros { get; set; }
        public decimal CON_Diferencia { get; set; }
        public DateTime CON_Fecha_Conciliacion { get; set; }
        public int ECO_Estado_Conciliacion { get; set; }
        public string ECO_Descripcion { get; set; } = string.Empty;

        public int Conciliados { get; set; }
        public int PendientesEnLibros { get; set; }
        public int PendientesEnBanco { get; set; }
        public int EnTransito { get; set; }
        public int DiferenciaMonto { get; set; }
        public int DiferenciaFecha { get; set; }

        public decimal TotalDepositosTransito { get; set; }
        public decimal TotalChequesCirculacion { get; set; }
        public decimal TotalErroresBancarios { get; set; }
        public decimal TotalAjustesContablesPendientes { get; set; }
        public decimal SaldoBancoAjustado { get; set; }
        public decimal SaldoLibrosAjustado { get; set; }

        public ConciliacionResponseDTO()
        {
        }
    }
}