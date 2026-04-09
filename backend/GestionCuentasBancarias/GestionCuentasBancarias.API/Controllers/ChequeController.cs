using GestionCuentasBancarias.Domain.DTOS.Cheque;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/cheques")]
    public class ChequeController : ControllerBase
    {
        private readonly IChequeService service;

        public ChequeController(IChequeService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCheques()
        {
            var cheques = await service.ObtenerCheques();
            return Ok(cheques);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCheque(int id)
        {
            var cheque = await service.ObtenerChequePorId(id);
            if (cheque == null) return NotFound();
            return Ok(cheque);
        }

        [HttpGet("cuenta/{cuentaId}")]
        public async Task<IActionResult> ObtenerChequesPorCuenta(int cuentaId)
        {
            var cheques = await service.ObtenerChequesPorCuenta(cuentaId);
            return Ok(cheques);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCheque([FromBody] CreateChequeDTO dto)
        {
            try
            {
                await service.CrearCheque(dto);
                return Ok(new { mensaje = "Cheque creado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoCheque(int id, [FromBody] UpdateDTOCheque dto)
        {
            try
            {
                var actualizado = await service.CambiarEstadoCheque(id, dto);
                if (!actualizado) return NotFound();
                return Ok(new { mensaje = "Estado del cheque actualizado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}