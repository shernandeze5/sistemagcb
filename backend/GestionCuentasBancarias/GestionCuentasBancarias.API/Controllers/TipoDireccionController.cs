using GestionCuentasBancarias.Domain.DTOS.TipoDireccion;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoDireccionController : ControllerBase
    {
        private readonly ITipoDireccionService _service;

        public TipoDireccionController(ITipoDireccionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var data = await _service.ObtenerTodosAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearTipoDireccionDTO dto)
        {
            var result = await _service.CrearAsync(dto);

            if (!result)
                return BadRequest("No se pudo crear el tipo de dirección.");

            return Ok("Tipo de dirección creado correctamente.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarTipoDireccionDTO dto)
        {
            var result = await _service.ActualizarAsync(id, dto);

            if (!result)
                return NotFound("No se encontró el tipo de dirección.");

            return Ok("Tipo de dirección actualizado correctamente.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _service.EliminarAsync(id);

            if (!result)
                return NotFound("No se encontró el tipo de dirección.");

            return Ok("Tipo de dirección eliminado correctamente.");
        }
    }
}
