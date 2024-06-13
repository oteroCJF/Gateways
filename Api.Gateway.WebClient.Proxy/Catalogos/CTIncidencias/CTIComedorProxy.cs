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
    public interface ICTIComedorProxy
    {
        Task<List<CTIComedorDto>> GetAllIncidenciasAsync();
        Task<List<CTIComedorDto>> GetIncidenciasByTipo(int incidencia);
        Task<List<CTIComedorDto>> GetNombresByTipo(string tipo);
        Task<CTIComedorDto> GetIncidenciaById(int id);
    }

    public class CTIComedorProxy : ICTIComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTIComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTIComedorDto>> GetAllIncidenciasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/icomedor");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIComedorDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTIComedorDto>> GetIncidenciasByTipo(int incidencia)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/icomedor/getIncidenciasByTipo/{incidencia}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIComedorDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTIComedorDto>> GetNombresByTipo(string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/icomedor/getNombresByTipo/{tipo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIComedorDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTIComedorDto> GetIncidenciaById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/icomedor/getIncidenciaById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTIComedorDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
