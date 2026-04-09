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

        [HttpPost("{id}/telefonos")]
        public async Task<IActionResult> AgregarTelefono(int id, [FromBody] CreateTelefonoPersonaExistenteDTO dto)
        {
            try
            {
                var created = await service.AgregarTelefonoAsync(id, dto);
                if (!created) return BadRequest(new { mensaje = "No se pudo agregar el teléfono." });

                return Ok(new { mensaje = "Teléfono agregado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("{id}/direcciones")]
        public async Task<IActionResult> AgregarDireccion(int id, [FromBody] CreateDireccionPersonaExistenteDTO dto)
        {
            try
            {
                var created = await service.AgregarDireccionAsync(id, dto);
                if (!created) return BadRequest(new { mensaje = "No se pudo agregar la dirección." });

                return Ok(new { mensaje = "Dirección agregada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("telefonos/{telefonoId}")]
        public async Task<IActionResult> ActualizarTelefono(int telefonoId, [FromBody] UpdateTelefonoPersonaDTO dto)
        {
            try
            {
                var updated = await service.ActualizarTelefonoAsync(telefonoId, dto);
                if (!updated) return NotFound();

                return Ok(new { mensaje = "Teléfono actualizado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("telefonos/{telefonoId}")]
        public async Task<IActionResult> EliminarTelefono(int telefonoId)
        {
            try
            {
                var deleted = await service.EliminarTelefonoLogicoAsync(telefonoId);
                if (!deleted) return NotFound();

                return Ok(new { mensaje = "Teléfono desactivado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("direcciones/{direccionId}")]
        public async Task<IActionResult> ActualizarDireccion(int direccionId, [FromBody] UpdateDireccionPersonaDTO dto)
        {
            try
            {
                var updated = await service.ActualizarDireccionAsync(direccionId, dto);
                if (!updated) return NotFound();

                return Ok(new { mensaje = "Dirección actualizada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("direcciones/{direccionId}")]
        public async Task<IActionResult> EliminarDireccion(int direccionId)
        {
            try
            {
                var deleted = await service.EliminarDireccionLogicoAsync(direccionId);
                if (!deleted) return NotFound();

                return Ok(new { mensaje = "Dirección desactivada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
