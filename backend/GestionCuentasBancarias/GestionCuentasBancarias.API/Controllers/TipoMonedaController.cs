using GestionCuentasBancarias.Domain.DTOS.TipoMoneda;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> ObtenerTiposMoneda()
        {
            var tipos = await service.ObtenerTiposMoneda();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObtenerTipoMoneda(int id)
        {
            var tipo = await service.ObtenerTipoMonedaPorId(id);

            if (tipo == null)
                return NotFound(new { mensaje = "Tipo de moneda no encontrado." });

            return Ok(tipo);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearTipoMoneda([FromBody] CreateTipoMonedaDTO dto)
        {
            try
            {
                var result = await service.CrearTipoMoneda(dto);

                return Ok(new
                {
                    mensaje = "Tipo de moneda creado correctamente.",
                    resultado = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarTipoMoneda(int id, [FromBody] UpdateTipoMonedaDTO dto)
        {
            try
            {
                var actualizado = await service.ActualizarTipoMoneda(id, dto);

                if (!actualizado)
                    return NotFound(new { mensaje = "Tipo de moneda no encontrado." });

                return Ok(new
                {
                    mensaje = "Tipo de moneda actualizado correctamente."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarTipoMoneda(int id)
        {
            var deleted = await service.EliminarTipoMoneda(id);

            if (!deleted)
                return NotFound(new { mensaje = "Tipo de moneda no encontrado." });

            return Ok(new
            {
                mensaje = "Tipo de moneda desactivado correctamente."
            });
        }

        [HttpPatch("{id}/reactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ReactivarTipoMoneda(int id)
        {
            var reactivado = await service.ReactivarTipoMoneda(id);

            if (!reactivado)
                return NotFound(new { mensaje = "Tipo de moneda no encontrado." });

            return Ok(new
            {
                mensaje = "Tipo de moneda reactivado correctamente."
            });
        }
    }
}