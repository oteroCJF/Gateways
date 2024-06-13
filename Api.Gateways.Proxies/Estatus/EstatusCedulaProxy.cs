using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusCedulas;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Estatus
{
    public interface IEstatusCedulaProxy
    {
        Task<List<EstatusDto>> GetAllEstatusCedulaAsync();
        Task<EstatusDto> GetECByIdAsync(int estatus);
        Task<List<FlujoServicioDto>> GetFlujoByServicio(int servicio, int estatusC, string flujo);
    }

    public class EstatusCedulaProxy : IEstatusCedulaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public EstatusCedulaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusCedulaAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/cedulas");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/cedulas/getECedulaById/{estatus}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/flujos/getFlujoByServicio/{servicio}/{estatusC}/{flujo}");
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
