using GestionCuentasBancarias.Domain.DTOS.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Domain.Interfaces.Repositories
{
    public interface IRolRepository
    {
        Task<IEnumerable<RolDTO>> ObtenerTodos();
        Task<RolDTO?> ObtenerPorId(int id);
        Task<int> Crear(CrearRolDTO dto);
        Task<bool> Actualizar(int id, ActualizarRolDTO dto);
        Task<bool> Desactivar(int id);
        Task<bool> Reactivar(int id);
    }
}
