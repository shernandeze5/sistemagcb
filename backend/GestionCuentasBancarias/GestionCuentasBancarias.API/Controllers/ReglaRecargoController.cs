using GestionCuentasBancarias.Domain.DTOS.ReglaRecargo;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/regla-recargo")]
    public class ReglaRecargoController : ControllerBase
    {
        private readonly IReglaRecargoService service;

        public ReglaRecargoController(IReglaRecargoService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CreateReglaRecargoDTO dto)
        {
            var id = await service.Crear(dto);
            return Ok(new { Id = id });
        }

        [HttpGet("cuenta/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var data = await service.ObtenerPorCuenta(id);
            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, UpdateReglaRecargoDTO dto)
        {
            await service.Actualizar(id, dto);
            return Ok("Actualizado");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await service.Eliminar(id);
            return Ok("Eliminado");
        }
    }
}
