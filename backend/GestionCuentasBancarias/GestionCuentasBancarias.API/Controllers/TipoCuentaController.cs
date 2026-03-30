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
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoCuenta([FromBody] CreateTipoCuentaDTO dto)
        {
            try
            {
                await service.CrearTipoCuenta(dto);
                return Ok(new { mensaje = "Tipo de cuenta creado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTipoCuenta(int id, [FromBody] UpdateTipoCuentaDTO dto)
        {
            try
            {
                await service.ActualizarTipoCuenta(id, dto);
                return Ok(new { mensaje = "Tipo de cuenta actualizado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTipoCuenta(int id)
        {
            var deleted = await service.EliminarTipoCuenta(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Tipo de cuenta desactivado correctamente." });
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarTipoCuenta(int id)
        {
            var reactivado = await service.ReactivarTipoCuenta(id);
            if (!reactivado) return NotFound();
            return Ok(new { mensaje = "Tipo de cuenta reactivado correctamente." });
        }
    }
}