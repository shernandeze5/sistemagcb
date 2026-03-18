using GestionCuentasBancarias.Business.Services;
using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoChequeController : ControllerBase
    {
        private readonly IEstadoChequeService _service;

        public EstadoChequeController(IEstadoChequeService service)
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
                return NotFound(new { mensaje = "Estado Cheque no encontrado." });

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEstadoChequeDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ESC_Descripcion))
                return BadRequest(new { mensaje = "La descripción no puede quedar vacia." });

            var resultado = await _service.CrearAsync(dto);

            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el nuevo Estado Cheque." });

            return Ok(new { mensaje = "Estado Cheque creado correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEstadoChequeDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ESC_Descripcion))
                return BadRequest(new { mensaje = "La descripción no puede quedar vacia." });

            var resultado = await _service.ActualizarAsync(id, dto);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo actualizar. El registro no fue encontrado." });

            return Ok(new { mensaje = "Estado Cheque actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            var resultado = await _service.EliminarLogicoAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "No se pudo eliminar. El registro no fue encontrado." });

            return Ok(new { mensaje = "Estado Cheque eliminado correctamente." });
        }
    }
}