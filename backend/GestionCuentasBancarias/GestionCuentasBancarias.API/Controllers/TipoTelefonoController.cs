using GestionCuentasBancarias.Domain.DTOS.TipoTelefono;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/tipotelefono")]
    public class TipoTelefonoController : ControllerBase
    {
        private readonly ITipoTelefonoService _service;

        public TipoTelefonoController(ITipoTelefonoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var data = await _service.ObtenerTodosAsync();
            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Crear([FromBody] CrearTipoTelefonoDTO dto)
        {
            var result = await _service.CrearAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarTipoTelefonoDTO dto)
        {
            var result = await _service.ActualizarAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _service.EliminarAsync(id);
            return Ok(result);
        }
    }
}