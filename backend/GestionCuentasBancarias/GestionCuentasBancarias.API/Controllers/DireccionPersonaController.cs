using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/direcciones-persona")]
    public class DireccionPersonaController : Controller
    {
        private readonly IDireccionPersonaService service;

        public DireccionPersonaController(IDireccionPersonaService service)
        {
            this.service = service;
        }

        [HttpGet("persona/{personaId}")]
        public async Task<IActionResult> ObtenerDireccionesPorPersona(int personaId)
        {
            try
            {
                var direcciones = await service.ObtenerPorPersonaAsync(personaId);
                return Ok(direcciones);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerDireccion(int id)
        {
            try
            {
                var direccion = await service.ObtenerPorIdAsync(id);
                if (direccion == null) return NotFound();
                return Ok(direccion);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearDireccion([FromBody] CreateDireccionPersonaDTO dto)
        {
            try
            {
                await service.CrearAsync(dto);
                return Ok(new { mensaje = "Dirección creada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarDireccion(int id, [FromBody] UpdateDireccionPersonaDTO dto)
        {
            try
            {
                var updated = await service.ActualizarAsync(id, dto);
                if (!updated) return NotFound();
                return Ok(new { mensaje = "Dirección actualizada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarDireccion(int id)
        {
            try
            {
                var deleted = await service.EliminarLogicoAsync(id);
                if (!deleted) return NotFound();
                return Ok(new { mensaje = "Dirección eliminada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
