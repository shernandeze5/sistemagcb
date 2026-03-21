using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoMovimientoController : ControllerBase
    {
        private readonly IEstadoMovimientoService _service;

        public EstadoMovimientoController(IEstadoMovimientoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var data = await _service.ObtenerTodosAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var data = await _service.ObtenerPorIdAsync(id);

            if (data == null)
                return NotFound(new { mensaje = "EstadoMovimiento no encontrado." });

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEstadoMovimientoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ESM_Descripcion))
                return BadRequest(new { mensaje = "La descripción es obligatoria." });

            var resultado = await _service.CrearAsync(dto);

            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el EstadoMovimiento." });

            return Ok(new { mensaje = "EstadoMovimiento creado correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEstadoMovimientoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ESM_Descripcion))
                return BadRequest(new { mensaje = "La descripción es obligatoria." });

            var resultado = await _service.ActualizarAsync(id, dto);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo actualizar. Registro no encontrado." });

            return Ok(new { mensaje = "EstadoMovimiento actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            var resultado = await _service.EliminarLogicoAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo eliminar. Registro no encontrado." });

            return Ok(new { mensaje = "EstadoMovimiento eliminado lógicamente." });
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> Reactivar(int id)
        {
            var resultado = await _service.Reactivar(id);
            if (!resultado)
                return NotFound(new { mensaje = "No se pudo reactivar. Registro no encontrado." });
            return Ok(new { mensaje = "EstadoMovimiento reactivado correctamente." });
        }
    }
}