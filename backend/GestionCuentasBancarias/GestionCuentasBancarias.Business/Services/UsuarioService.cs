using GestionCuentasBancarias.Domain.DTOS.Usuario;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<UsuarioDTO>> ObtenerTodos()
        {
            return await repository.ObtenerTodos();
        }

        public async Task<UsuarioDTO?> ObtenerPorId(int id)
        {
            return await repository.ObtenerPorId(id);
        }

        public async Task<int> Crear(CrearUsuarioDTO dto)
        {
            var existente = await repository.ObtenerPorEmail(dto.USU_EMAIL);

            if (existente != null)
            {
                throw new Exception("Ya existe un usuario registrado con ese correo electrónico.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.USU_PASSWORD);

            return await repository.Crear(dto, passwordHash);
        }

        public async Task<bool> Actualizar(int id, ActualizarUsuarioDTO dto)
        {
            return await repository.Actualizar(id, dto);
        }

        public async Task<bool> CambiarPassword(int id, CambiarPasswordDTO dto)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);

            return await repository.CambiarPassword(id, passwordHash);
        }

        public async Task<bool> Desactivar(int id)
        {
            return await repository.Desactivar(id);
        }

        public async Task<bool> Reactivar(int id)
        {
            return await repository.Reactivar(id);
        }

        public async Task<bool> ActualizarUltimoAcceso(int id)
        {
            return await repository.ActualizarUltimoAcceso(id);
        }
    }
}
