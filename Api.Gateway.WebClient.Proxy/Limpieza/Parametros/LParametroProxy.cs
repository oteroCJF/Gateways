using Api.Gateway.Models.Parametrizacion.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Limpieza.Variables
{
    public interface ILParametroProxy
    {
        Task<List<ParametroDto>> GetAllVariables();
        Task<ParametroDto> GetVariableById(int variable);
        Task<List<ParametroDto>> GetVariablesTipoIncidencia();
        Task<int> GetVariableIdByTipoIncidencia(string abreviacion, string valor);
    }
    public class LParametroProxy : ILParametroProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public LParametroProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<ParametroDto>> GetAllVariables()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/variables");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ParametroDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<ParametroDto> GetVariableById(int variable)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/variables/getVariableById/{variable}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ParametroDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<List<ParametroDto>> GetVariablesTipoIncidencia()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/variables/getVariablesTipoIncidencia");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ParametroDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<int> GetVariableIdByTipoIncidencia(string abreviacion, string valor)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/variables/getVariableById/{abreviacion}/{valor}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
