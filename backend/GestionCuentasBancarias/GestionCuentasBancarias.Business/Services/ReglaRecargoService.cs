using GestionCuentasBancarias.Domain.DTOS.ReglaRecargo;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    using GestionCuentasBancarias.Domain.DTOS.ReglaRecargo;
    using GestionCuentasBancarias.Domain.Interfaces.Repositories;
    using GestionCuentasBancarias.Domain.Interfaces.Services;

    public class ReglaRecargoService : IReglaRecargoService
    {
        private readonly IReglaRecargoRepository repo;

        public ReglaRecargoService(IReglaRecargoRepository repo)
        {
            this.repo = repo;
        }

        public Task<int> Crear(CreateReglaRecargoDTO dto)
            => repo.Crear(dto);

        public Task<IEnumerable<ResponseReglaRecargoDTO>> ObtenerPorCuenta(int cuentaId)
            => repo.ObtenerPorCuenta(cuentaId);

        public Task Actualizar(int id, UpdateReglaRecargoDTO dto)
            => repo.Actualizar(id, dto);

        public Task Eliminar(int id)
            => repo.Eliminar(id);
    }
}
