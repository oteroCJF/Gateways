using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Models.Catalogos.DTOs.Indemnizaciones;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTIndemnizaciones
{
    public interface ICTIndemnizacionProxy
    {
        Task<List<CTIndemnizacionDto>> GetAllIndemnizacionesAsync();
        Task<List<CTIndemnizacionDto>> GetIndemnizacionByIncidencia(int incidencia);
        Task<CTIndemnizacionDto> GetIndemnizacionById(int id);
    }

    public class CTIndemnizacionProxy : ICTIndemnizacionProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTIndemnizacionProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTIndemnizacionDto>> GetAllIndemnizacionesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/indemnizacion");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIndemnizacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTIndemnizacionDto>> GetIndemnizacionByIncidencia(int incidencia)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/indemnizacion/getIndemnizacionByIncidencia/{incidencia}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIndemnizacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTIndemnizacionDto> GetIndemnizacionById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/indemnizacion/getIndemnizacionById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTIndemnizacionDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
