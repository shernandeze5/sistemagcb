using GestionCuentasBancarias.Domain.DTOS.Usuario;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService service;

        public UsuarioController(IUsuarioService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUsuarioDTO dto)
        {
            try
            {
                var id = await service.Crear(dto);

                return Ok(new
                {
                    mensaje = "Usuario creado correctamente.",
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
                return NotFound(new { mensaje = "Usuario no encontrado." });

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarUsuarioDTO dto)
        {
            try
            {
                var actualizado = await service.Actualizar(id, dto);

                if (!actualizado)
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return Ok(new
                {
                    mensaje = "Usuario actualizado correctamente."
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

        [HttpPatch("{id}/password")]
        public async Task<IActionResult> CambiarPassword(int id, [FromBody] CambiarPasswordDTO dto)
        {
            try
            {
                var actualizado = await service.CambiarPassword(id, dto);

                if (!actualizado)
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return Ok(new
                {
                    mensaje = "Contraseña actualizada correctamente."
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
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return Ok(new
                {
                    mensaje = "Usuario desactivado correctamente."
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
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return Ok(new
                {
                    mensaje = "Usuario reactivado correctamente."
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

        [HttpPatch("{id}/ultimo-acceso")]
        public async Task<IActionResult> ActualizarUltimoAcceso(int id)
        {
            try
            {
                var actualizado = await service.ActualizarUltimoAcceso(id);

                if (!actualizado)
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return Ok(new
                {
                    mensaje = "Último acceso actualizado correctamente."
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
