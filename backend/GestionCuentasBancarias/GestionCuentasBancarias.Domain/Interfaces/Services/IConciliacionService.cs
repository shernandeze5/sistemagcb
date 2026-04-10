using GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IConciliacionService
    {
        Task<int> Procesar(ProcesarConciliacionDTO dto);

        Task<ConciliacionResponseDTO?> ObtenerPorId(int conciliacionId);
        Task<IEnumerable<DetalleConciliacionResponseDTO>> ObtenerDetalle(int conciliacionId);
        Task<IEnumerable<ConciliacionResponseDTO>> ObtenerPorCuenta(int cuentaId);
        Task<IEnumerable<ConciliacionResponseDTO>> ObtenerTodas();

        Task RegistrarEnLibros(int detalleId);
        Task MarcarEnTransito(int detalleId);
        Task AceptarManual(int detalleId);
        Task RecalcularEstado(int conciliacionId);
        Task Cerrar(int conciliacionId);
    }
}
