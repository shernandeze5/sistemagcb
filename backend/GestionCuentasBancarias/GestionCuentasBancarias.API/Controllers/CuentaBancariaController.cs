using GestionCuentasBancarias.Domain.DTOS.CuentaBancaria;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/cuentas-bancarias")]
    public class CuentaBancariaController : Controller
    {
        private readonly ICuentaBancariaService service;

        public CuentaBancariaController(ICuentaBancariaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCuentas()
        {
            var cuentas = await service.ObtenerCuentas();
            return Ok(cuentas);
        }

        // GET api/cuentas-bancarias/banco/1
        [HttpGet("banco/{bancoId}")]
        public async Task<IActionResult> ObtenerCuentasPorBanco(int bancoId)
        {
            var cuentas = await service.ObtenerCuentasPorBanco(bancoId);
            return Ok(cuentas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCuenta(int id)
        {
            var cuenta = await service.ObtenerCuentaPorId(id);
            if (cuenta == null) return NotFound();
            return Ok(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta([FromBody] CreateCuentaBancariaDTO dto)
        {
            try
            {
                await service.CrearCuenta(dto);
                return Ok(new { mensaje = "Cuenta bancaria creada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCuenta(int id, [FromBody] UpdateCuentaBancariaDTO dto)
        {
            try
            {
                var updated = await service.ActualizarCuenta(id, dto);
                if (!updated) return NotFound();
                return Ok(new { mensaje = "Cuenta bancaria actualizada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Baja lógica → CUB_Estado = 'I'
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCuenta(int id)
        {
            var deleted = await service.EliminarCuenta(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Cuenta bancaria desactivada correctamente." });
        }

        // Reactivación → CUB_Estado = 'A'
        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarCuenta(int id)
        {
            var reactivado = await service.ReactivarCuenta(id);
            if (!reactivado) return NotFound();
            return Ok(new { mensaje = "Cuenta bancaria reactivada correctamente." });
        }
    }
}
