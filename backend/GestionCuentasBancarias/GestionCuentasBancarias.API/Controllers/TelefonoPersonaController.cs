using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/telefonos-persona")]
    public class TelefonoPersonaController : Controller
    {
        private readonly ITelefonoPersonaService service;

        public TelefonoPersonaController(ITelefonoPersonaService service)
        {
            this.service = service;
        }

        [HttpGet("persona/{personaId}")]
        public async Task<IActionResult> ObtenerTelefonosPorPersona(int personaId)
        {
            try
            {
                var telefonos = await service.ObtenerPorPersonaAsync(personaId);
                return Ok(telefonos);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTelefono(int id)
        {
            try
            {
                var telefono = await service.ObtenerPorIdAsync(id);
                if (telefono == null) return NotFound();
                return Ok(telefono);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearTelefono([FromBody] CreateTelefonoPersonaDTO dto)
        {
            try
            {
                await service.CrearAsync(dto);
                return Ok(new { mensaje = "Teléfono creado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTelefono(int id, [FromBody] UpdateTelefonoPersonaDTO dto)
        {
            try
            {
                var updated = await service.ActualizarAsync(id, dto);
                if (!updated) return NotFound();
                return Ok(new { mensaje = "Teléfono actualizado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTelefono(int id)
        {
            try
            {
                var deleted = await service.EliminarLogicoAsync(id);
                if (!deleted) return NotFound();
                return Ok(new { mensaje = "Teléfono eliminado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
