using GestionCuentasBancarias.Domain.DTOS.Auth;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GestionCuentasBancarias.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;

        public AuthController(IAuthService service)
        {
            this.service = service;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var data = await service.Login(dto);

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
    }
}
