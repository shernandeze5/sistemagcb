using GestionCuentasBancarias.Domain.DTOS.ConversionMoneda;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Business.Services
{
    public class ConversionMonedaService : IConversionMonedaService
    {
        private readonly IConversionMonedaRepository repository;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string apiKey;

        private const string API_URL = "https://v6.exchangerate-api.com/v6";

        private static readonly JsonSerializerOptions JSON_OPTIONS = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ConversionMonedaService(
            IConversionMonedaRepository repository,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            this.repository = repository;
            this.httpClientFactory = httpClientFactory;
            this.apiKey = configuration["ExchangeRateApiKey"]
                ?? throw new InvalidOperationException(
                    "No se encontró ExchangeRateApiKey en appsettings.json. " +
                    "Regístrate gratis en https://www.exchangerate-api.com/");
        }

        public Task<List<ResponseConversionMonedaDTO>> ObtenerConversiones() =>
            repository.ObtenerConversiones();

        public Task<ResponseConversionMonedaDTO> ObtenerConversionPorId(int id) =>
            repository.ObtenerConversionPorId(id);

        public Task<ResponseConversionMonedaDTO> ObtenerTasaVigente(
            int monedaOrigen, int monedaDestino, string fecha) =>
            repository.ObtenerTasaVigente(monedaOrigen, monedaDestino, fecha);

        public Task CrearConversion(CreateConversionMonedaDTO dto) =>
            repository.CrearConversion(dto);

        public Task<bool> ActualizarConversion(int id, UpdateConversionMonedaDTO dto) =>
            repository.ActualizarConversion(id, dto);

        public Task<bool> EliminarConversion(int id) =>
            repository.EliminarConversion(id);

        public Task<bool> ReactivarConversion(int id) =>
            repository.ReactivarConversion(id);

        // ── Conexión a ExchangeRate-API ──────────────────────────────
        // Usa TMO_Codigo_ISO (ej. USD, GTQ, EUR) para la consulta.
        // Si ya existe la tasa del día en BD no llama a la API.
        // POST api/conversiones-moneda/desde-api?origen=41&destino=42
        public async Task<ResponseConversionMonedaDTO> ObtenerTasaDesdeApi(int monedaOrigenId, int monedaDestinoId)
        {
            var hoy = DateTime.Now.ToString("yyyy-MM-dd");

            // 1. Verificar si ya existe
            var tasaExistente = await repository.ObtenerTasaVigente(
                monedaOrigenId, monedaDestinoId, hoy);

            if (tasaExistente != null)
                return tasaExistente;

            // 2. Obtener ISO correctamente (ÚNICA FORMA)
            string codigoOrigen = await repository.ObtenerCodigoISO(monedaOrigenId);
            string codigoDestino = await repository.ObtenerCodigoISO(monedaDestinoId);

            if (string.IsNullOrEmpty(codigoOrigen) || string.IsNullOrEmpty(codigoDestino))
                throw new InvalidOperationException("Las monedas no tienen código ISO configurado.");

            // 3. Llamar API
            var client = httpClientFactory.CreateClient();
            string url = $"{API_URL}/{apiKey}/pair/{codigoOrigen}/{codigoDestino}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    $"ExchangeRate-API devolvió error {(int)response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ExchangeRateApiResponse>(json, JSON_OPTIONS)
                ?? throw new InvalidOperationException("Respuesta vacía de API");

            if (apiResponse.result != "success")
                throw new InvalidOperationException($"Error API: {apiResponse.result}");

            // 4. Guardar
            var dto = new CreateConversionMonedaDTO
            {
                TMO_Tipo_Moneda = monedaOrigenId,
                TMO_Tipo_Moneda_Destino = monedaDestinoId,
                COM_Tasa_Cambio = apiResponse.conversion_rate,
                COM_Fecha_Vigencia = DateTime.Now.Date,
                COM_Fuente = "API"
            };

            try
            {
                await repository.CrearConversion(dto);
            }
            catch { }

            // 5. Retornar
            return await repository.ObtenerTasaVigente(
                monedaOrigenId, monedaDestinoId, hoy)
                ?? throw new InvalidOperationException("No se pudo recuperar la tasa.");
        }
    }
}
