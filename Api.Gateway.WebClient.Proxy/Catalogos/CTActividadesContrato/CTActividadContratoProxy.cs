using Api.Gateway.Models.Catalogos.DTOs.ActividadesContrato;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTActividadesContrato
{
    public interface ICTActividadContratoProxy
    {
        Task<List<CTActividadContratoDto>> GetAllActividadesContrato();
        Task<CTActividadContratoDto> GetActividadByIdAsync(int id);
    }
    
    public class CTActividadContratoProxy : ICTActividadContratoProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTActividadContratoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTActividadContratoDto>> GetAllActividadesContrato()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/actividades");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTActividadContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTActividadContratoDto> GetActividadByIdAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/actividades/getActividadById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTActividadContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
