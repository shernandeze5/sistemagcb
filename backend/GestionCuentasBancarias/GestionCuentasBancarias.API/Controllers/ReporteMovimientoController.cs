using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/reportes/movimientos")]
    public class ReporteMovimientoController : ControllerBase
    {
        private readonly IReporteMovimientoService service;

        public ReporteMovimientoController(IReporteMovimientoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReporte(
            [FromQuery] int? cuentaId,
            [FromQuery] int? tipoMovimientoId,
            [FromQuery] int? medioMovimientoId,
            [FromQuery] int? estadoMovimientoId,
            [FromQuery] int? personaId)
        {
            try
            {
                var data = await service.ObtenerReporte(
                    cuentaId,
                    tipoMovimientoId,
                    medioMovimientoId,
                    estadoMovimientoId,
                    personaId
                );

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
