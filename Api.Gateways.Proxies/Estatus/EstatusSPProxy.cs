using Api.Gateway.Models.Estatus.DTOs;
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
    public interface IEstatusSPProxy
    {
        Task<List<EstatusDto>> GetAllEstatusSPagoAsync();
        Task<EstatusDto> GetESPagoByIdAsync(int estatus);
        Task<List<FlujoBasicosDto>> GetFlujoESPagoAsync(int servicio, int estatus);
    }

    public class EstatusSPProxy : IEstatusSPProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public EstatusSPProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusSPagoAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/solicitudesPago");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EstatusDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<EstatusDto> GetESPagoByIdAsync(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/solicitudesPago/getESPagoById/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EstatusDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<FlujoBasicosDto>> GetFlujoESPagoAsync(int servicio, int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/solicitudesPago/getSPByServicio/{servicio}/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FlujoBasicosDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
