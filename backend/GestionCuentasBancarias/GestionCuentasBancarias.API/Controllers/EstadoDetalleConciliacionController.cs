using GestionCuentasBancarias.Domain.DTOS.DetalleConciliacion;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/estados-detalle-conciliacion")]
    public class EstadoDetalleConciliacionController : ControllerBase
    {
        private readonly IEstadoDetalleConciliacionService service;

        public EstadoDetalleConciliacionController(IEstadoDetalleConciliacionService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObtenerEstadosDetalleConciliacion()
        {
            var estados = await service.ObtenerEstadosDetalleConciliacion();
            return Ok(estados);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObtenerEstadoDetalleConciliacion(int id)
        {
            var estado = await service.ObtenerEstadoDetalleConciliacionPorId(id);

            if (estado == null)
                return NotFound();

            return Ok(estado);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearEstadoDetalleConciliacion([FromBody] CreateEstadoDetalleConciliacionDTO dto)
        {
            await service.CrearEstadoDetalleConciliacion(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarEstadoDetalleConciliacion(int id, [FromBody] UpdateEstadoDetalleConciliacionDTO dto)
        {
            await service.ActualizarEstadoDetalleConciliacion(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarEstadoDetalleConciliacion(int id)
        {
            await service.EliminarEstadoDetalleConciliacion(id);
            return Ok();
        }

        [HttpPatch("{id}/reactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ReactivarEstadoDetalleConciliacion(int id)
        {
            var reactivado = await service.ReactivarEstadoDetalleConciliacion(id);
            if (!reactivado) return NotFound();
            return Ok();
        }
    }
}
