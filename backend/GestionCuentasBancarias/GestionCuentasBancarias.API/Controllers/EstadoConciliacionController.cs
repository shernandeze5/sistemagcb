using GestionCuentasBancarias.Domain.DTOS.Conciliacion;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/estados-conciliacion")]
    public class EstadoConciliacionController : ControllerBase
    {
        private readonly IEstadoConciliacionService service;

        public EstadoConciliacionController(IEstadoConciliacionService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObtenerEstadosConciliacion()
        {
            var estados = await service.ObtenerEstadosConciliacion();
            return Ok(estados);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObtenerEstadoConciliacion(int id)
        {
            var estado = await service.ObtenerEstadoConciliacionPorId(id);

            if (estado == null)
                return NotFound();

            return Ok(estado);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearEstadoConciliacion([FromBody] CreateEstadoConciliacionDTO dto)
        {
            await service.CrearEstadoConciliacion(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarEstadoConciliacion(int id, [FromBody] UpdateEstadoConciliacionDTO dto)
        {
            await service.ActualizarEstadoConciliacion(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarEstadoConcliacion(int id)
        {
            await service.EliminarEstadoConciliacion(id);
            return Ok();
        }

        [HttpPatch("{id}/reactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ReactivarEstadoConciliacion(int id)
        {
            var reactivado = await service.ReactivarEstadoConciliacion(id);
            if (!reactivado) return NotFound();
            return Ok();
        }
    }
}
