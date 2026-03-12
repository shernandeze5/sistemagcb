using GestionCuentasBancarias.Domain.DTOS.Banco;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/bancos")]
    public class BancoController : ControllerBase
    {
        private readonly IBancoService service;

        public BancoController(IBancoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerBancos()
        {
            var bancos = await service.ObtenerBancos();
            return Ok(bancos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerBanco(int id)
        {
            var banco = await service.ObtenerBancoPorId(id);

            if (banco == null)
                return NotFound();

            return Ok(banco);
        }

        [HttpPost]
        public async Task<IActionResult> CrearBanco([FromBody] CreateBancoDTO dto)
        {
            await service.CrearBanco(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarBanco(int id, [FromBody] UpdataBancoDTO dto)
        {
            await service.ActualizarBanco(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarBanco(int id)
        {
            await service.EliminarBanco(id);
            return Ok();
        }
    }
}
