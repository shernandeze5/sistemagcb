using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/reportes/chequeras")]
    public class ReporteChequeraController : ControllerBase
    {
        private readonly IReporteChequeraService service;

        public ReporteChequeraController(IReporteChequeraService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReporte(
            [FromQuery] int? cuentaId,
            [FromQuery] string? estado)
        {
            try
            {
                var data = await service.ObtenerReporte(cuentaId, estado);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}