using GestionCuentasBancarias.Domain.DTOS.TasaInteres;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/tasa-interes")]
    public class TasaInteresController : ControllerBase
    {
        private readonly ITasaInteresService service;

        public TasaInteresController(ITasaInteresService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTasas()
        {
            var tasas = await service.ObtenerTasas();
            return Ok(tasas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTasa(int id)
        {
            var tasa = await service.ObtenerTasaPorId(id);
            if (tasa == null) return NotFound();
            return Ok(tasa);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTasa([FromBody] CreateTasaInteresDTO dto)
        {
            try
            {
                await service.CrearTasa(dto);
                return Ok(new { mensaje = "Tasa de interés creada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTasa(int id, [FromBody] UpdateTasaInteresDTO dto)
        {
            try
            {
                var updated = await service.ActualizarTasa(id, dto);
                if (!updated) return NotFound();
                return Ok(new { mensaje = "Tasa de interés actualizada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTasa(int id)
        {
            var deleted = await service.EliminarTasa(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Tasa de interés desactivada correctamente." });
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarTasa(int id)
        {
            var reactivado = await service.ReactivarTasa(id);
            if (!reactivado) return NotFound();
            return Ok(new { mensaje = "Tasa de interés reactivada correctamente." });
        }
    }
}
