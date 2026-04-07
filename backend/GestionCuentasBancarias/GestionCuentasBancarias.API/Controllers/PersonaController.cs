using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/persona")]
    public class PersonaController : Controller
    {
        private readonly IPersonaService service;

        public PersonaController(IPersonaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPersonas()
        {
            var personas = await service.ObtenerTodosAsync();
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPersona(int id)
        {
            var persona = await service.ObtenerPorIdAsync(id);
            if (persona == null) return NotFound();
            return Ok(persona);
        }

        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> ObtenerPersonaDetalle(int id)
        {
            var persona = await service.ObtenerDetallePorIdAsync(id);
            if (persona == null) return NotFound();
            return Ok(persona);
        }

        [HttpPost]
        public async Task<IActionResult> CrearPersona([FromBody] CreatePersonaDTO dto)
        {
            try
            {
                var creada = await service.CrearAsync(dto);
                return Ok(new
                {
                    mensaje = "Persona creada correctamente.",
                    personaId = creada.PER_Persona
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPersona(int id, [FromBody] UpdatePersonaDTO dto)
        {
            try
            {
                var updated = await service.ActualizarAsync(id, dto);
                if (!updated) return NotFound();
                return Ok(new { mensaje = "Persona actualizada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPersona(int id)
        {
            try
            {
                var deleted = await service.EliminarLogicoAsync(id);
                if (!deleted) return NotFound();
                return Ok(new { mensaje = "Persona desactivada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
