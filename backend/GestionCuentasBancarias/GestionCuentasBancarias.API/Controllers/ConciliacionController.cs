using GestionCuentasBancarias.Domain.DTOS.ConciliacionBancaria;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/conciliacion")]
    public class ConciliacionController : ControllerBase
    {
        private readonly IConciliacionService service;

        public ConciliacionController(IConciliacionService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerConciliaciones()
        {
            var data = await service.ObtenerTodas();
            return Ok(data);
        }

        [HttpPost("procesar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Procesar([FromForm] ProcesarConciliacionDTO dto)
        {
            try
            {
                var id = await service.Procesar(dto);
                return Ok(new
                {
                    mensaje = "Conciliación procesada correctamente.",
                    id
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

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerConciliacion(int id)
        {
            try
            {
                var data = await service.ObtenerPorId(id);
                if (data == null) return NotFound();

                return Ok(data);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> ObtenerDetalle(int id)
        {
            try
            {
                var data = await service.ObtenerDetalle(id);
                return Ok(data);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("cuenta/{id}")]
        public async Task<IActionResult> ObtenerPorCuenta(int id)
        {
            try
            {
                var data = await service.ObtenerPorCuenta(id);
                return Ok(data);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("detalle/{id}/registrar-en-libros")]
        public async Task<IActionResult> RegistrarEnLibros(int id)
        {
            try
            {
                await service.RegistrarEnLibros(id);
                return Ok(new { mensaje = "Movimiento registrado en libros correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPatch("detalle/{id}/marcar-transito")]
        public async Task<IActionResult> MarcarEnTransito(int id)
        {
            try
            {
                await service.MarcarEnTransito(id);
                return Ok(new { mensaje = "Detalle marcado en tránsito correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPatch("detalle/{id}/aceptar-manual")]
        public async Task<IActionResult> AceptarManual(int id)
        {
            try
            {
                await service.AceptarManual(id);
                return Ok(new { mensaje = "Detalle aceptado manualmente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPatch("{id}/recalcular-estado")]
        public async Task<IActionResult> RecalcularEstado(int id)
        {
            try
            {
                await service.RecalcularEstado(id);
                return Ok(new { mensaje = "Estado de conciliación recalculado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPatch("{id}/cerrar")]
        public async Task<IActionResult> Cerrar(int id)
        {
            try
            {
                await service.Cerrar(id);
                return Ok(new { mensaje = "Conciliación cerrada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}