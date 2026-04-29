using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/reportes/cheques")]
    public class ReporteChequeController : ControllerBase
    {
        private readonly IReporteChequeService service;

        public ReporteChequeController(IReporteChequeService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReporte(
            [FromQuery] int? cuentaId,
            [FromQuery] int? estadoChequeId,
            [FromQuery] int? chequeraId,
            [FromQuery] int? personaId)
        {
            try
            {
                var data = await service.ObtenerReporte(
                    cuentaId,
                    estadoChequeId,
                    chequeraId,
                    personaId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
