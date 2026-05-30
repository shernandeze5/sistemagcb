using GestionCuentasBancarias.Domain.DTOS.EstadoCuenta;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> ObtenerEstadosCuenta()
        {
            var estados = await service.ObtenerEstadosCuenta();
            return Ok(estados);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObtenerEstadoCuenta(int id)
        {
            var estado = await service.ObtenerEstadoCuentaPorId(id);
            if (estado == null) return NotFound();
            return Ok(estado);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearEstadoCuenta([FromBody] CreateEstadoCuentaDTO dto)
        {
            try
            {
                await service.CrearEstadoCuenta(dto);
                return Ok(new { mensaje = "Estado de cuenta creado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarEstadoCuenta(int id, [FromBody] UpdateEstadoCuentaDTO dto)
        {
            try
            {
                await service.ActualizarEstadoCuenta(id, dto);
                return Ok(new { mensaje = "Estado de cuenta actualizado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarEstadoCuenta(int id)
        {
            var deleted = await service.EliminarEstadoCuenta(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Estado de cuenta desactivado correctamente." });
        }

        [HttpPatch("{id}/reactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ReactivarEstadoCuenta(int id)
        {
            var reactivado = await service.ReactivarEstadoCuenta(id);
            if (!reactivado) return NotFound();
            return Ok(new { mensaje = "Estado de cuenta reactivado correctamente." });
        }
    }
}