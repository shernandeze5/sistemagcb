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

        [HttpGet("cuenta/{cuentaId}")]
        public async Task<IActionResult> ObtenerPorCuenta(int cuentaId)
        {
            return Ok(await service.ObtenerPorCuenta(cuentaId));
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CreateMovimientoDTO dto)
        {
            var id = await service.CrearMovimiento(dto);
            return Ok(new { MOV_Movimiento = id });
        }
    }
}