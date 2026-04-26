using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/reportes/cuentas-bancarias")]
    public class ReporteCuentaBancariaController : ControllerBase
    {
        private readonly IReporteCuentaBancariaService service;

        public ReporteCuentaBancariaController(IReporteCuentaBancariaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReporte(
            [FromQuery] int? bancoId,
            [FromQuery] int? tipoCuentaId,
            [FromQuery] int? tipoMonedaId,
            [FromQuery] int? estadoCuentaId,
            [FromQuery] string? estadoRegistro)
        {
            try
            {
                var data = await service.ObtenerReporte(
                    bancoId,
                    tipoCuentaId,
                    tipoMonedaId,
                    estadoCuentaId,
                    estadoRegistro
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
