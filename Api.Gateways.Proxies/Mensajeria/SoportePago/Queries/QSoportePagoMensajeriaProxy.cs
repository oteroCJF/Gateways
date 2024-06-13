using Api.Gateway.Models.Incidencias.Mensajeria.Commands;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Proxies.Mensajeria.SoportePago.Commands;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Mensajeria.SoportePago.Queries
{
    public interface IQSoportePagoMensajeriaProxy
    {
        Task<List<MSoportePagoDto>> GetSoportePagoByCedula(int cedula);
        Task<List<MSoportePagoDto>> GetGuiasPendientes(int cedula);
        Task<MSoportePagoDto> GetSoportePagoById(int soporte);
    }

    public class QSoportePagoMensajeriaProxy : IQSoportePagoMensajeriaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QSoportePagoMensajeriaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<MSoportePagoDto>> GetSoportePagoByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/soportePago/getSoportePagoByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MSoportePagoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<MSoportePagoDto>> GetGuiasPendientes(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/soportePago/getGuiasPendientes/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MSoportePagoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<MSoportePagoDto> GetSoportePagoById(int soporte)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/soportePago/getSoportePagoById/{soporte}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MSoportePagoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
