using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusCedulas;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Estatus
{
    public interface IEstatusCedulaProxy
    {
        Task<List<EstatusDto>> GetAllEstatusCedulaAsync();
        Task<EstatusDto> GetECByIdAsync(int estatus);
        Task<List<FlujoServicioDto>> GetFlujoByServicio(int servicio, int estatusC, string flujo);
    }
    public class EstatusCedulaProxy : IEstatusCedulaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public EstatusCedulaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusCedulaAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/cedulas");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EstatusDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<EstatusDto> GetECByIdAsync(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/cedulas/getECedulaById/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EstatusDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<FlujoServicioDto>> GetFlujoByServicio(int servicio, int estatusC, string flujo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/cedulas/getFlujoByServicio/{servicio}/{estatusC}/{flujo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FlujoServicioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
