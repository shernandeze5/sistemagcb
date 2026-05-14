using GestionCuentasBancarias.Domain.DTOS.Auth;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository repository;
        private readonly IConfiguration configuration;

        public AuthService(IAuthRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
        }

        public async Task<AuthResponseDTO> Login(LoginDTO dto)
        {
            var usuario = await repository.ObtenerUsuarioPorEmail(dto.USU_EMAIL);

            if (usuario == null)
                throw new Exception("Correo o contraseña incorrectos.");

            if (usuario.USU_ESTADO != "A")
                throw new Exception("El usuario se encuentra inactivo.");

            var passwordValida = BCrypt.Net.BCrypt.Verify(
                dto.USU_PASSWORD,
                usuario.USU_PASSWORD
            );

            if (!passwordValida)
                throw new Exception("Correo o contraseña incorrectos.");

            await repository.ActualizarUltimoAcceso(usuario.USU_USUARIO);

            var usuarioSesion = new UsuarioSesionDTO
            {
                USU_USUARIO = usuario.USU_USUARIO,
                ROL_ROL = usuario.ROL_ROL,
                ROL_NOMBRE = usuario.ROL_NOMBRE,
                NombreCompleto = usuario.NombreCompleto,
                USU_EMAIL = usuario.USU_EMAIL,
                USU_ESTADO = usuario.USU_ESTADO
            };

            var token = GenerarToken(usuarioSesion);

            return new AuthResponseDTO
            {
                Mensaje = "Inicio de sesión correcto.",
                Token = token,
                Usuario = usuarioSesion
            };
        }

        private string GenerarToken(UsuarioSesionDTO usuario)
        {
            var jwtKey = configuration["Jwt:Key"]
                ?? throw new Exception("No se encontró Jwt:Key en appsettings.json.");

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.USU_USUARIO.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario.USU_EMAIL),
                new Claim(ClaimTypes.Role, usuario.ROL_NOMBRE),
                new Claim("rolId", usuario.ROL_ROL.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
