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
            if (banco == null) return NotFound();
            return Ok(banco);
        }

        [HttpPost]
        public async Task<IActionResult> CrearBanco([FromBody] CreateBancoDTO dto)
        {
            try
            {
                await service.CrearBanco(dto);
                return Ok(new { mensaje = "Banco creado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarBanco(int id, [FromBody] UpdataBancoDTO dto)
        {
            try
            {
                var updated = await service.ActualizarBanco(id, dto);
                if (!updated) return NotFound();
                return Ok(new { mensaje = "Banco actualizado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Baja lógica → BAN_Estado = 'I'
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarBanco(int id)
        {
            var deleted = await service.EliminarBanco(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Banco desactivado correctamente." });
        }

        // Reactivación → BAN_Estado = 'A'
        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarBanco(int id)
        {
            var reactivado = await service.ReactivarBanco(id);
            if (!reactivado) return NotFound();
            return Ok(new { mensaje = "Banco reactivado correctamente." });
        }
    }
}