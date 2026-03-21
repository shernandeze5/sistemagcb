using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/estados-cheque")]
    public class EstadoChequeController : ControllerBase
    {
        private readonly IEstadoChequeService service;

        public EstadoChequeController(IEstadoChequeService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEstadosCheque()
        {
            var estados = await service.ObtenerEstadosCheque();
            return Ok(estados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEstadoCheque(int id)
        {
            var estado = await service.ObtenerEstadoChequePorId(id);

            if (estado == null)
                return NotFound();

            return Ok(estado);
        }

        [HttpPost]
        public async Task<IActionResult> CrearEstadoCheque([FromBody] CreateEstadoChequeDTO dto)
        {
            await service.CrearEstadoCheque(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEstadoCheque(int id, [FromBody] UpdateEstadoChequeDTO dto)
        {
            await service.ActualizarEstadoCheque(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEstadoCheque(int id)
        {
            await service.EliminarEstadoCheque(id);
            return Ok();
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarEstadoCheque(int id)
        {
            var reactivado = await service.ReactivarEstadoCheque(id);
            if (!reactivado) return NotFound();
            return Ok();
        }
    }
}
