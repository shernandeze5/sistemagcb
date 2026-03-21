using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.DTOS.MedioMovimiento;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedioMovimientoController : ControllerBase
    {
        private readonly IMedioMovimientoService _service;

        public MedioMovimientoController(IMedioMovimientoService service)
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
                return NotFound(new { mensaje = "MedioMovimiento no encontrado." });

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearMedioMovimientoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MEM_Descripcion))
                return BadRequest(new { mensaje = "La descripción es obligatoria." });

            var resultado = await _service.CrearAsync(dto);

            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el MedioMovimiento." });

            return Ok(new { mensaje = "MedioMovimiento creado correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarMedioMovimientoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MEM_Descripcion))
                return BadRequest(new { mensaje = "La descripción es obligatoria." });

            var resultado = await _service.ActualizarAsync(id, dto);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo actualizar. Registro no encontrado." });

            return Ok(new { mensaje = "MedioMovimiento actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            var resultado = await _service.EliminarLogicoAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo eliminar. Registro no encontrado." });

            return Ok(new { mensaje = "MedioMovimiento eliminado lógicamente." });
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> Reactivar(int id)
        {
            var resultado = await _service.Reactivar(id);
            if (!resultado)
                return NotFound(new { mensaje = "No se pudo reactivar. Registro no encontrado." });
            return Ok(new { mensaje = "MedioMovimiento reactivado correctamente." });
        }
    }
}