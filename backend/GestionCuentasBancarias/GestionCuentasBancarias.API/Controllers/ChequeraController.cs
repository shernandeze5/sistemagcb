using GestionCuentasBancarias.Domain.DTOS.Chequera;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/chequera")]
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
            try
            {
                var resultado = await _service.CrearAsync(dto);

                if (!resultado)
                    return BadRequest(new { mensaje = "No se pudo crear la chequera." });

                return Ok(new { mensaje = "Chequera creada correctamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}/actualizar")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarChequeraDTO dto)
        {
            try
            {
                var resultado = await _service.ActualizarAsync(id, dto);

                if (!resultado)
                    return NotFound(new { mensaje = "No se pudo actualizar. Registro no encontrado." });

                return Ok(new { mensaje = "Chequera actualizada correctamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            var resultado = await _service.EliminarLogicoAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo eliminar. Registro no encontrado." });

            return Ok(new { mensaje = "Chequera desactivada correctamente." });
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
