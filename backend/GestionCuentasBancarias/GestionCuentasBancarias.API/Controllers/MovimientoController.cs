using GestionCuentasBancarias.Domain.DTOS;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/movimiento")]
    public class MovimientoController : ControllerBase
    {
        private readonly IMovimientoService service;

        public MovimientoController(IMovimientoService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CreateMovimientoDTO dto)
        {
            try
            {
                var id = await service.Crear(dto);
                return Ok(new
                {
                    mensaje = "Movimiento creado correctamente.",
                    id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var data = await service.ObtenerTodos();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var data = await service.ObtenerPorId(id);

            if (data == null)
                return NotFound(new { mensaje = "Movimiento no encontrado." });

            return Ok(data);
        }

        [HttpGet("cuenta/{id}")]
        public async Task<IActionResult> ObtenerPorCuenta(int id)
        {
            try
            {
                var data = await service.ObtenerPorCuenta(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        [HttpPatch("{id}/anular")]
        public async Task<IActionResult> Anular(int id)
        {
            try
            {
                await service.Anular(id);
                return Ok(new
                {
                    mensaje = "Movimiento anulado correctamente."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }
    }
}