using GestionCuentasBancarias.Domain.DTOS.Chequera;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChequeraController : ControllerBase
    {
        private readonly IChequeraService _service;

        public ChequeraController(IChequeraService service)
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
                return NotFound(new { mensaje = "Chequera no encontrada." });

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearChequeraDTO dto)
        {
            if (dto.CUB_Cuenta <= 0)
                return BadRequest(new { mensaje = "La cuenta es obligatoria." });

            if (string.IsNullOrWhiteSpace(dto.CHQ_Serie))
                return BadRequest(new { mensaje = "La serie es obligatoria." });

            if (dto.CHQ_Numero_Desde <= 0)
                return BadRequest(new { mensaje = "El número desde debe ser mayor que cero." });

            if (dto.CHQ_Numero_Hasta <= 0)
                return BadRequest(new { mensaje = "El número hasta debe ser mayor que cero." });

            if (dto.CHQ_Numero_Hasta < dto.CHQ_Numero_Desde)
                return BadRequest(new { mensaje = "El número hasta no puede ser menor al número desde." });

            if (dto.CHQ_Fecha_Recepcion == default)
                return BadRequest(new { mensaje = "La fecha de recepción es obligatoria." });

            var resultado = await _service.CrearAsync(dto);

            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear la Chequera." });

            return Ok(new { mensaje = "Chequera creada correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarChequeraDTO dto)
        {
            if (dto.CUB_Cuenta <= 0)
                return BadRequest(new { mensaje = "La cuenta es obligatoria." });

            if (string.IsNullOrWhiteSpace(dto.CHQ_Serie))
                return BadRequest(new { mensaje = "La serie es obligatoria." });

            if (dto.CHQ_Numero_Desde <= 0)
                return BadRequest(new { mensaje = "El número desde debe ser mayor que cero." });

            if (dto.CHQ_Numero_Hasta <= 0)
                return BadRequest(new { mensaje = "El número hasta debe ser mayor que cero." });

            if (dto.CHQ_Numero_Hasta < dto.CHQ_Numero_Desde)
                return BadRequest(new { mensaje = "El número hasta no puede ser menor al número desde." });

            if (dto.CHQ_Ultimo_Usado < 0)
                return BadRequest(new { mensaje = "El último usado no puede ser negativo." });

            if (string.IsNullOrWhiteSpace(dto.CHQ_Estado))
                return BadRequest(new { mensaje = "El estado es obligatorio." });

            if (dto.CHQ_Fecha_Recepcion == default)
                return BadRequest(new { mensaje = "La fecha de recepción es obligatoria." });

            var resultado = await _service.ActualizarAsync(id, dto);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo actualizar. Registro no encontrado." });

            return Ok(new { mensaje = "Chequera actualizada correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            var resultado = await _service.EliminarLogicoAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo eliminar. Registro no encontrado." });

            return Ok(new { mensaje = "Chequera eliminada lógicamente." });
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> Reactivar(int id)
        {
            var resultado = await _service.Reactivar(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo reactivar. Registro no encontrado." });

            return Ok(new { mensaje = "Chequera reactivada correctamente." });
        }
    }
}
