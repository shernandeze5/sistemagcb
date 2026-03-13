using GestionCuentasBancarias.Domain.DTOS.TipoMoneda;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/tipos-moneda")]
    public class TipoMonedaController : ControllerBase
    {
        private readonly ITipoMonedaService service;

        public TipoMonedaController(ITipoMonedaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTiposMoneda()
        {
            var tipos = await service.ObtenerTiposMoneda();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTipoMoneda(int id)
        {
            var tipo = await service.ObtenerTipoMonedaPorId(id);

            if (tipo == null)
                return NotFound();

            return Ok(tipo);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoMoneda([FromBody] CreateTipoMonedaDTO dto)
        {
            await service.CrearTipoMoneda(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTipoMoneda(int id, [FromBody] UpdateTipoMonedaDTO dto)
        {
            await service.ActualizarTipoMoneda(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTipoMoneda(int id)
        {
            await service.EliminarTipoMoneda(id);
            return Ok();
        }

    }
}
