using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTIncidencias
{
    public interface ICTIncidenciaProxy
    {
        Task<List<CTIncidenciaDto>> GetAllIncidenciasAsync();
        Task<List<CTIncidenciaDto>> GetIncidenciasByServicio(int servicio);
        Task<CTIncidenciaDto> GetIncidenciaById(int incidencia);
    }

    public class CTIncidenciaProxy : ICTIncidenciaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTIncidenciaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTIncidenciaDto>> GetAllIncidenciasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/incidencias");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTIncidenciaDto>> GetIncidenciasByServicio(int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/incidencias/getIncidenciasByServicio/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTIncidenciaDto> GetIncidenciaById(int incidencia)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/incidencias/getIncidenciaById/{incidencia}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
