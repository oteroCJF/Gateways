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
    public interface ICTILimpiezaProxy
    {
        Task<List<CTILimpiezaDto>> GetAllIncidenciasAsync();
        Task<List<CTILimpiezaDto>> GetIncidenciasByTipo(int incidencia);
        Task<List<CTILimpiezaDto>> GetNombresByTipo(string tipo);
        Task<CTILimpiezaDto> GetIncidenciaById(int id);
    }

    public class CTILimpiezaProxy : ICTILimpiezaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTILimpiezaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTILimpiezaDto>> GetAllIncidenciasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/ilimpieza");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTILimpiezaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTILimpiezaDto>> GetIncidenciasByTipo(int incidencia)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/ilimpieza/getIncidenciasByTipo/{incidencia}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTILimpiezaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTILimpiezaDto>> GetNombresByTipo(string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/ilimpieza/getNombresByTipo/{tipo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTILimpiezaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTILimpiezaDto> GetIncidenciaById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/ilimpieza/getIncidenciaById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTILimpiezaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
