using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusCedulas;
using Api.Gateway.Models.Estatus.DTOs.EstatusOficios;
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
    public interface IEstatusOficioProxy
    {
        Task<List<EstatusDto>> GetAllEstatusOficiosAsync();
        Task<EstatusDto> GetEOficioById(int estatus);
        Task<List<FlujoOficiosDto>> GetFlujoByOficio(int servicio, int estatusO);
    }

    public class OficioProxy : IEstatusOficioProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public OficioProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusOficiosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/oficios");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EstatusDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<EstatusDto> GetEOficioById(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/oficios/getEOficioById/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EstatusDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<FlujoOficiosDto>> GetFlujoByOficio(int servicio, int estatusO)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/flujosOficios/getFlujoByServicio/{servicio}/{estatusO}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FlujoOficiosDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
