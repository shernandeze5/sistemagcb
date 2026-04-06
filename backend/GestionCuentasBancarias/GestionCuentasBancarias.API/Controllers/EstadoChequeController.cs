using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/estados-cheque")]
    public class EstadoChequeController : ControllerBase
    {
        private readonly IEstadoChequeService service;

        public EstadoChequeController(IEstadoChequeService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEstadosCheque()
        {
            var estados = await service.ObtenerEstadosCheque();
            return Ok(estados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEstadoCheque(int id)
        {
            var estado = await service.ObtenerEstadoChequePorId(id);
            if (estado == null) return NotFound();
            return Ok(estado);
        }

        [HttpPost]
        public async Task<IActionResult> CrearEstadoCheque([FromBody] CreateEstadoChequeDTO dto)
        {
            try
            {
                await service.CrearEstadoCheque(dto);
                return Ok(new { mensaje = "Estado de cheque creado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEstadoCheque(int id, [FromBody] UpdateEstadoChequeDTO dto)
        {
            try
            {
                await service.ActualizarEstadoCheque(id, dto);
                return Ok(new { mensaje = "Estado de cheque actualizado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEstadoCheque(int id)
        {
            var deleted = await service.EliminarEstadoCheque(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Estado de cheque desactivado correctamente." });
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarEstadoCheque(int id)
        {
            var reactivado = await service.ReactivarEstadoCheque(id);
            if (!reactivado) return NotFound();
            return Ok(new { mensaje = "Estado de cheque reactivado correctamente." });
        }
    }
}