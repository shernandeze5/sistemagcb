using GestionCuentasBancarias.Business.Services;
using GestionCuentasBancarias.Domain.DTOS.DetalleConciliacion;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoDetalleConciliacionController : ControllerBase
    {
        private readonly IEstadoDetalleConciliacionService _service;

        public EstadoDetalleConciliacionController(IEstadoDetalleConciliacionService service)
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
                return NotFound(new { mensaje = "Estado Detalle Conciliación no encontrado." });

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEstadoDetalleConciliacionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EDC_Descripcion))
                return BadRequest(new { mensaje = "La descripción no puede quedar vacia." });

            var resultado = await _service.CrearAsync(dto);

            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el nuevo Estado Detalle Conciliación." });

            return Ok(new { mensaje = "Estado Detalle Conciliación creado correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEstadoDetalleConciliacionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EDC_Descripcion))
                return BadRequest(new { mensaje = "La descripción no puede quedar vacia." });

            var resultado = await _service.ActualizarAsync(id, dto);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo actualizar. El registro no fue encontrado." });

            return Ok(new { mensaje = "Estado Detalle Conciliación actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            var resultado = await _service.EliminarLogicoAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo eliminar. El registro no fue encontrado." });

            return Ok(new { mensaje = "Estado Detalle Conciliación eliminado correctamente." });
        }
    }
}