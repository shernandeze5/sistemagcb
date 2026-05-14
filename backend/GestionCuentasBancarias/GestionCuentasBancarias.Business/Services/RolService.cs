using GestionCuentasBancarias.Domain.DTOS.Rol;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository repository;

        public RolService(IRolRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<RolDTO>> ObtenerTodos()
        {
            return await repository.ObtenerTodos();
        }

        public async Task<RolDTO?> ObtenerPorId(int id)
        {
            return await repository.ObtenerPorId(id);
        }

        public async Task<int> Crear(CrearRolDTO dto)
        {
            return await repository.Crear(dto);
        }

        public async Task<bool> Actualizar(int id, ActualizarRolDTO dto)
        {
            return await repository.Actualizar(id, dto);
        }

        public async Task<bool> Desactivar(int id)
        {
            return await repository.Desactivar(id);
        }

        public async Task<bool> Reactivar(int id)
        {
            return await repository.Reactivar(id);
        }
    }
}
