using GestionCuentasBancarias.Domain.DTOS.NewFolder;
using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/tipopersona")]
    public class TipoPersonaController : ControllerBase
    {
        private readonly ITipoPersonaService service;

        public TipoPersonaController(ITipoPersonaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTiposPersona()
        {
            var tipos = await service.ObtenerTodosAsync();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTipoPersona(int id)
        {
            var tipo = await service.ObtenerPorIdAsync(id);

            if (tipo == null)
                return NotFound();

            return Ok(tipo);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoPersona([FromBody] CrearTipoPersonaDTO dto)
        {
            var creado = await service.CrearAsync(dto);

            if (!creado)
                return BadRequest();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTipoPersona(int id, [FromBody] ActualizarTipoPersonaDTO dto)
        {
            var actualizado = await service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTipoPersona(int id)
        {
            var eliminado = await service.EliminarLogicoAsync(id);

            if (!eliminado)
                return NotFound();

            return Ok();
        }
    }

}
