using GestionCuentasBancarias.Domain.DTOS.InteresFrecuencia;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/interes-frecuencia")]
    public class InteresFrecuenciaController : ControllerBase
    {
        private readonly IinteresFrecuenciaService service;

        public InteresFrecuenciaController(IinteresFrecuenciaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerFrecuencias()
        {
            var frecuencias = await service.ObtenerFrecuencias();
            return Ok(frecuencias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerFrecuencia(int id)
        {
            var frecuencia = await service.ObtenerFrecuenciaPorId(id);
            if (frecuencia == null) return NotFound();
            return Ok(frecuencia);
        }

        [HttpPost]
        public async Task<IActionResult> CrearFrecuencia([FromBody] CreateInteresFrecuenciaDTO dto)
        {
            try
            {
                await service.CrearFrecuencia(dto);
                return Ok(new { mensaje = "Frecuencia de interés creada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarFrecuencia(int id, [FromBody] UpdateInteresFrecuenciaDTO dto)
        {
            try
            {
                var updated = await service.ActualizarFrecuencia(id, dto);
                if (!updated) return NotFound();
                return Ok(new { mensaje = "Frecuencia de interés actualizada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarFrecuencia(int id)
        {
            var deleted = await service.EliminarFrecuencia(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Frecuencia de interés desactivada correctamente." });
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarFrecuencia(int id)
        {
            var reactivado = await service.ReactivarFrecuencia(id);
            if (!reactivado) return NotFound();
            return Ok(new { mensaje = "Frecuencia de interés reactivada correctamente." });
        }
    }
}
