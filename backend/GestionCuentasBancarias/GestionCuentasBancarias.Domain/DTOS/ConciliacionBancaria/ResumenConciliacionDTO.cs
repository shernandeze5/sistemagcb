namespace GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria
{
    public class ResumenConciliacionDTO
    {
        public int Conciliados { get; set; }
        public int PendientesEnLibros { get; set; }
        public int PendientesEnBanco { get; set; }
        public int EnTransito { get; set; }
        public int DiferenciaMonto { get; set; }
        public int DiferenciaFecha { get; set; }

        public ResumenConciliacionDTO() { }
    }
}