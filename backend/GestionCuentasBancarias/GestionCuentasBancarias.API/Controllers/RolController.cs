using GestionCuentasBancarias.Domain.DTOS.Rol;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolController : ControllerBase
    {
        private readonly IRolService service;

        public RolController(IRolService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearRolDTO dto)
        {
            try
            {
                var id = await service.Crear(dto);

                return Ok(new
                {
                    mensaje = "Rol creado correctamente.",
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
                return NotFound(new { mensaje = "Rol no encontrado." });

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarRolDTO dto)
        {
            try
            {
                var actualizado = await service.Actualizar(id, dto);

                if (!actualizado)
                    return NotFound(new { mensaje = "Rol no encontrado." });

                return Ok(new
                {
                    mensaje = "Rol actualizado correctamente."
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

        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            try
            {
                var actualizado = await service.Desactivar(id);

                if (!actualizado)
                    return NotFound(new { mensaje = "Rol no encontrado." });

                return Ok(new
                {
                    mensaje = "Rol desactivado correctamente."
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

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> Reactivar(int id)
        {
            try
            {
                var actualizado = await service.Reactivar(id);

                if (!actualizado)
                    return NotFound(new { mensaje = "Rol no encontrado." });

                return Ok(new
                {
                    mensaje = "Rol reactivado correctamente."
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
