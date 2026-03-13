using GestionCuentasBancarias.Domain.DTOS.TipoTelefono;
using GestionCuentasBancarias.Domain.Interfaces.Services;
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
        public async Task<IActionResult> Get()
        {
            var data = await _service.ObtenerTodosAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearTipoTelefonoDTO dto)
        {
            var result = await _service.CrearAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, ActualizarTipoTelefonoDTO dto)
        {
            var result = await _service.ActualizarAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _service.EliminarAsync(id);
            return Ok(result);
        }
    }
}
