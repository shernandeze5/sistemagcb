using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChequeController : ControllerBase
    {
        private readonly IChequeService _service;

        public ChequeController(IChequeService service)
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
                return NotFound(new { mensaje = "Cheque no encontrado." });

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearChequeDTO dto)
        {
            if (dto.MOV_Movimiento <= 0)
                return BadRequest(new { mensaje = "El movimiento es obligatorio." });

            if (string.IsNullOrWhiteSpace(dto.CHE_Numero_Cheque))
                return BadRequest(new { mensaje = "El número de cheque es obligatorio." });

            if (dto.ESC_Estado_Cheque <= 0)
                return BadRequest(new { mensaje = "El estado del cheque es obligatorio." });

            var resultado = await _service.CrearAsync(dto);

            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el Cheque." });

            return Ok(new { mensaje = "Cheque creado correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarChequeDTO dto)
        {
            if (dto.MOV_Movimiento <= 0)
                return BadRequest(new { mensaje = "El movimiento es obligatorio." });

            if (string.IsNullOrWhiteSpace(dto.CHE_Numero_Cheque))
                return BadRequest(new { mensaje = "El número de cheque es obligatorio." });

            if (dto.ESC_Estado_Cheque <= 0)
                return BadRequest(new { mensaje = "El estado del cheque es obligatorio." });

            var resultado = await _service.ActualizarAsync(id, dto);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo actualizar. Registro no encontrado." });

            return Ok(new { mensaje = "Cheque actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _service.EliminarAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo eliminar. Registro no encontrado." });

            return Ok(new { mensaje = "Cheque eliminado correctamente." });
        }
    }
}