using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Estatus
{
    public interface IEstatusSPProxy
    {
        Task<List<EstatusDto>> GetAllEstatusSPagoAsync();
        Task<EstatusDto> GetESPagoByIdAsync(int estatus);
        Task<List<FlujoBasicosDto>> GetFlujoESPagoAsync(int servicio, int estatus);
    }

    public class EstatusSPProxy : IEstatusSPProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public EstatusSPProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusSPagoAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/solicitudesPago");
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
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/solicitudesPago/getESPagoById/{estatus}");
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
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/solicitudesPago/getSPByServicio/{servicio}/{estatus}");
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
