using GestionCuentasBancarias.Domain.DTOS.ReglaRecargo;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/reglas-recargo")]
    public class ReglaRecargoController : ControllerBase
    {
        private readonly IReglaRecargoService service;

        public ReglaRecargoController(IReglaRecargoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReglas()
        {
            var reglas = await service.ObtenerReglas();
            return Ok(reglas);
        }

        // Obtener reglas por cuenta — útil para cargar al abrir una cuenta
        [HttpGet("cuenta/{cuentaId}")]
        public async Task<IActionResult> ObtenerReglasPorCuenta(int cuentaId)
        {
            var reglas = await service.ObtenerReglasPorCuenta(cuentaId);
            return Ok(reglas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerRegla(int id)
        {
            var regla = await service.ObtenerReglaPorId(id);
            if (regla == null) return NotFound();
            return Ok(regla);
        }

        [HttpPost]
        public async Task<IActionResult> CrearRegla([FromBody] CreateReglaRecargoDTO dto)
        {
            try
            {
                await service.CrearRegla(dto);
                return Ok(new { mensaje = "Regla de recargo creada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarRegla(int id, [FromBody] UpdateReglaRecargoDTO dto)
        {
            try
            {
                var updated = await service.ActualizarRegla(id, dto);
                if (!updated) return NotFound();
                return Ok(new { mensaje = "Regla de recargo actualizada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarRegla(int id)
        {
            var deleted = await service.EliminarRegla(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Regla de recargo desactivada correctamente." });
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarRegla(int id)
        {
            var reactivado = await service.ReactivarRegla(id);
            if (!reactivado) return NotFound();
            return Ok(new { mensaje = "Regla de recargo reactivada correctamente." });
        }
    }
}
