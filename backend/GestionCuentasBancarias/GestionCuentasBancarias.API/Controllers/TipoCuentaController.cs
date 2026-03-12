using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.DTOS.TipoCuenta;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/tipos-cuenta")]
    public class TipoCuentaController : ControllerBase
    {
        private readonly ITipoCuentaService service;

        public TipoCuentaController(ITipoCuentaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTiposCuenta()
        {
            var tipos = await service.ObtenerTiposCuenta();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTipoCuenta(int id)
        {
            var tipo = await service.ObtenerTipoCuentaPorId(id);

            if (tipo == null)
                return NotFound();

            return Ok(tipo);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoCuenta([FromBody] CreateTipoCuentaDTO dto)
        {
            await service.CrearTipoCuenta(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTipoCuenta(int id, [FromBody] UpdateTipoCuentaDTO dto)
        {
            await service.ActualizarTipoCuenta(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTipoCuenta(int id)
        {
            await service.EliminarTipoCuenta(id);
            return Ok();
        }

    }
}
