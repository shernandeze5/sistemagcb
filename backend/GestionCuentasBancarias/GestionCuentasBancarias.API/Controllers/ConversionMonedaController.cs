using GestionCuentasBancarias.Domain.DTOS.ConversionMoneda;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/conversiones-moneda")]
    public class ConversionMonedaController : ControllerBase
    {
        private readonly IConversionMonedaService service; 

        public ConversionMonedaController(IConversionMonedaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerConversiones()
        {
            var conversiones = await service.ObtenerConversiones();
            return Ok(conversiones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerConversion(int id)
        {
            var conversion = await service.ObtenerConversionPorId(id);
            if (conversion == null) return NotFound();
            return Ok(conversion);
        }

        // Obtiene la tasa vigente para un par de monedas en una fecha
        // GET api/conversiones-moneda/vigente?origen=1&destino=2&fecha=2026-03-23
        [HttpGet("vigente")]
        public async Task<IActionResult> ObtenerTasaVigente(
            [FromQuery] int origen,
            [FromQuery] int destino,
            [FromQuery] string fecha)
        {
            var tasa = await service.ObtenerTasaVigente(origen, destino, fecha);
            if (tasa == null)
                return NotFound(new { mensaje = "No se encontró tasa vigente para ese par de monedas en esa fecha." });
            return Ok(tasa);
        }

        // Consulta la API externa y guarda la tasa del día automáticamente
        // POST api/conversiones-moneda/desde-api?origen=1&destino=2
        [HttpPost("desde-api")]
        public async Task<IActionResult> ObtenerTasaDesdeApi(
            [FromQuery] int origen,
            [FromQuery] int destino)
        {
            try
            {
                var tasa = await service.ObtenerTasaDesdeApi(origen, destino);
                return Ok(tasa);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearConversion([FromBody] CreateConversionMonedaDTO dto)
        {
            try
            {
                await service.CrearConversion(dto);
                return Ok(new { mensaje = "Conversión de moneda registrada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarConversion(int id, [FromBody] UpdateConversionMonedaDTO dto)
        {
            try
            {
                var updated = await service.ActualizarConversion(id, dto);
                if (!updated) return NotFound();
                return Ok(new { mensaje = "Conversión de moneda actualizada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarConversion(int id)
        {
            var deleted = await service.EliminarConversion(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Conversión de moneda desactivada correctamente." });
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarConversion(int id)
        {
            var reactivado = await service.ReactivarConversion(id);
            if (!reactivado) return NotFound();
            return Ok(new { mensaje = "Conversión de moneda reactivada correctamente." });
        }
    }
}
