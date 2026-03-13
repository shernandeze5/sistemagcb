using GestionCuentasBancarias.Domain.DTOS.EstadoCuenta;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/estados-cuenta")]
    public class EstadoCuentaController : ControllerBase
    {
        private readonly IEstadoCuentaService service;

        public EstadoCuentaController(IEstadoCuentaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEstadosCuenta()
        {
            var estados = await service.ObtenerEstadosCuenta();
            return Ok(estados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEstadoCuenta(int id)
        {
            var estado = await service.ObtenerEstadoCuentaPorId(id);

            if (estado == null)
                return NotFound();

            return Ok(estado);
        }

        [HttpPost]
        public async Task<IActionResult> CrearEstadoCuenta([FromBody] CreateEstadoCuentaDTO dto)
        {
            await service.CrearEstadoCuenta(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEstadoCuenta(int id, [FromBody] UpdateEstadoCuentaDTO dto)
        {
            await service.ActualizarEstadoCuenta(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEstadoCuenta(int id)
        {
            await service.EliminarEstadoCuenta(id);
            return Ok();
        }
    }
}
