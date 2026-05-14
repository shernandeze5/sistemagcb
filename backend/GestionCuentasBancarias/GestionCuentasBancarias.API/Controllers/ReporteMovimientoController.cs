using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/reportes/movimientos")]
    [Authorize(Roles = "Administrador,Contador,Auxiliar")]
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
            [FromQuery] int? personaId,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var data = await service.ObtenerReporte(
                    cuentaId,
                    tipoMovimientoId,
                    medioMovimientoId,
                    estadoMovimientoId,
                    personaId,
                    fechaInicio,
                    fechaFin
                );

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

    }
}
