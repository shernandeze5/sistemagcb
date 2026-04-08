using GestionCuentasBancarias.Business.Services;
using GestionCuentasBancarias.Domain.DTOS.AplicacionInteres;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/aplicacion-intereses")]
    public class AplicacionInteresController : ControllerBase
    {
        private readonly IAplicacionInteresService service;

        public AplicacionInteresController(IAplicacionInteresService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Aplicar([FromBody] CreateAplicacionInteresDTO dto)
        {
            try
            {
                int movimientoId = await service.AplicarInteres(dto);
                return Ok(new { MovimientoId = movimientoId, Mensaje = "Interés aplicado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("cuenta/{id}")]
        public async Task<IActionResult> ObtenerPorCuenta(int id)
        {
            var result = await service.ObtenerPorCuenta(id);
            return Ok(result);
        }

        [HttpGet("tasa/{id}")]
        public async Task<IActionResult> ObtenerPorTasa(int id)
        {
            var result = await service.ObtenerPorTasa(id);
            return Ok(result);
        }
    }
}
