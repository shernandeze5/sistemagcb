using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientoController : ControllerBase
    {
        private readonly IMovimientoService _service;

        public MovimientoController(IMovimientoService service)
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
                return NotFound(new { mensaje = "Movimiento no encontrado." });

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearMovimientoDTO dto)
        {
            if (dto.CUB_Cuenta <= 0)
                return BadRequest(new { mensaje = "La cuenta es obligatoria." });

            if (dto.PER_Persona <= 0)
                return BadRequest(new { mensaje = "La persona es obligatoria." });

            if (dto.TIM_Tipo_Movimiento <= 0)
                return BadRequest(new { mensaje = "El tipo de movimiento es obligatorio." });

            if (dto.MEM_Medio_Movimiento <= 0)
                return BadRequest(new { mensaje = "El medio de movimiento es obligatorio." });

            if (dto.ESM_Estado_Movimiento <= 0)
                return BadRequest(new { mensaje = "El estado de movimiento es obligatorio." });

            if (string.IsNullOrWhiteSpace(dto.MOV_Numero_Referencia))
                return BadRequest(new { mensaje = "El número de referencia es obligatorio." });

            if (string.IsNullOrWhiteSpace(dto.MOV_Descripcion))
                return BadRequest(new { mensaje = "La descripción es obligatoria." });

            if (dto.MOV_Monto <= 0)
                return BadRequest(new { mensaje = "El monto debe ser mayor que cero." });

            var resultado = await _service.CrearAsync(dto);

            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el Movimiento." });

            return Ok(new { mensaje = "Movimiento creado correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarMovimientoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MOV_Descripcion))
                return BadRequest(new { mensaje = "La descripción es obligatoria." });

            if (dto.ESM_Estado_Movimiento <= 0)
                return BadRequest(new { mensaje = "El estado de movimiento es obligatorio." });

            var resultado = await _service.ActualizarAsync(id, dto);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo actualizar. Registro no encontrado." });

            return Ok(new { mensaje = "Movimiento actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            var resultado = await _service.EliminarLogicoAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo eliminar. Registro no encontrado." });

            return Ok(new { mensaje = "Movimiento eliminado lógicamente." });
        }
    }
}