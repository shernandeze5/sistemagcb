using GestionCuentasBancarias.Domain.DTOS.AplicacionInteres;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class AplicacionInteresService : IAplicacionInteresService
    {
        private readonly IAplicacionInteresRepository repository;

        public AplicacionInteresService(IAplicacionInteresRepository repository)
        {
            this.repository = repository;
        }

        public Task<int> AplicarInteres(CreateAplicacionInteresDTO dto)
        {
            return repository.AplicarInteres(dto);
        }

        public Task<IEnumerable<ResponseAplicacionInteresDTO>> ObtenerPorCuenta(int cuentaId)
        {
            return repository.ObtenerPorCuenta(cuentaId);
        }

        public Task<IEnumerable<ResponseAplicacionInteresDTO>> ObtenerPorTasa(int tasaId)
        {
            return repository.ObtenerPorTasa(tasaId);
        }

        public async Task AplicarInteresesAutomaticos()
        {
            var tasas = await repository.ObtenerTasasActivas();

            foreach (var tasa in tasas)
            {
                if (!DebeAplicar(tasa.Frecuencia))
                    continue;

                string periodo = DateTime.Now.ToString("yyyy-MM");

                try
                {
                    await repository.AplicarInteres(new CreateAplicacionInteresDTO
                    {
                        TIN_Tasa_Interes = tasa.TIN_Tasa_Interes,
                        Periodo = periodo
                    });

                    await repository.AplicarRecargoAutomatico(tasa.CUB_Cuenta);
                }
                catch
                {
                    // ignorar errores controlados
                }
            }
        }

        private bool DebeAplicar(string frecuencia)
        {
            var hoy = DateTime.Now;

            return frecuencia switch
            {
                "Mensual" => true,
                "Trimestral" => hoy.Month % 3 == 0,
                "Anual" => hoy.Month == 12,
                _ => false
            };
        }


    }
}
