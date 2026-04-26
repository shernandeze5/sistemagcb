using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/reportes/conciliaciones")]
    public class ReporteConciliacionController : ControllerBase
    {
        private readonly IReporteConciliacionService service;

        public ReporteConciliacionController(IReporteConciliacionService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReporte(
            [FromQuery] int? cuentaId,
            [FromQuery] string? periodo,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] int? estadoConciliacionId)
        {
            try
            {
                var data = await service.ObtenerReporte(
                    cuentaId,
                    periodo,
                    fechaInicio,
                    fechaFin,
                    estadoConciliacionId
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
