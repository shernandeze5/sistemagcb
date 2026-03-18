using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.DTOS.MedioMovimiento;

namespace GestionCuentasBancarias.Domain.Interfaces.Services
{
    public interface IMedioMovimientoService
    {
        Task<IEnumerable<MedioMovimientoDTO>> ObtenerTodosAsync();

        Task<MedioMovimientoDTO?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(CrearMedioMovimientoDTO dto);

        Task<bool> ActualizarAsync(int id, ActualizarMedioMovimientoDTO dto);

        Task<bool> EliminarLogicoAsync(int id);
    }
}
