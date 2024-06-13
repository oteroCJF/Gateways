using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTIncidencias
{
    public interface ICTIFumigacionProxy
    {
        Task<List<CTIFumigacionDto>> GetAllIncidenciasAsync();
        Task<List<CTIFumigacionDto>> GetIncidenciasByTipo(int incidencia);
        Task<List<CTIFumigacionDto>> GetNombresByTipo(string tipo);
        Task<CTIFumigacionDto> GetIncidenciaById(int id);
    }

    public class CTIFumigacionProxy : ICTIFumigacionProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTIFumigacionProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTIFumigacionDto>> GetAllIncidenciasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/iFumigacion");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIFumigacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTIFumigacionDto>> GetIncidenciasByTipo(int incidencia)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/iFumigacion/getIncidenciasByTipo/{incidencia}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIFumigacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTIFumigacionDto>> GetNombresByTipo(string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/iFumigacion/getNombresByTipo/{tipo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIFumigacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTIFumigacionDto> GetIncidenciaById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/iFumigacion/getIncidenciaById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTIFumigacionDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
