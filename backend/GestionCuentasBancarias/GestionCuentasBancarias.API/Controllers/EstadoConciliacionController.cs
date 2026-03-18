using GestionCuentasBancarias.Business.Services;
using GestionCuentasBancarias.Domain.DTOS.Conciliacion;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoConciliacionController : ControllerBase
    {
        private readonly IEstadoConciliacionService _service;

        public EstadoConciliacionController(IEstadoConciliacionService service)
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
                return NotFound(new { mensaje = "Estado Conciliación no encontrado." });

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEstadoConciliacionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ECO_Descripcion))
                return BadRequest(new { mensaje = "La descripción no puede quedar vacia." });

            var resultado = await _service.CrearAsync(dto);

            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el nuevo Estado Conciliación." });

            return Ok(new { mensaje = "Estado Conciliación creado correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEstadoConciliacionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ECO_Descripcion))
                return BadRequest(new { mensaje = "La descripción no puede quedar vacia." });

            var resultado = await _service.ActualizarAsync(id, dto);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo actualizar. El registro no fue encontrado." });

            return Ok(new { mensaje = "Estado Conciliación actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            var resultado = await _service.EliminarLogicoAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo eliminar. El registro no fue encontrado." });

            return Ok(new { mensaje = "Estado Conciliación eliminado correctamente." });
        }
    }
}