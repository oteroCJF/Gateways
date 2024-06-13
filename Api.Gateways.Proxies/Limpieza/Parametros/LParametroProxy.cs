using Api.Gateway.Models.Parametrizacion.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Limpieza.Variables
{
    public interface ILVariableProxy
    {
        Task<List<ParametroDto>> GetAllVariables();
        Task<List<ParametroDto>> GetVariablesTipoIncidencia();
        Task<int> GetVariableIdByIncidencia(string abreviacion);
        Task<int> GetVariableIdByTipoIncidencia(string abreviacion, string valor);
        Task<ParametroDto> GetVariableById(int variable);
    }

    public class LParametroProxy : ILVariableProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public LParametroProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<ParametroDto>> GetAllVariables()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/variables");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ParametroDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<List<ParametroDto>> GetVariablesTipoIncidencia()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/variables/getVariablesTipoIncidencia");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/variables/getIdByVariables/{abreviacion}/{valor}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<int> GetVariableIdByIncidencia(string abreviacion)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/variables/getIdByIncidencia/{abreviacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<ParametroDto> GetVariableById(int variable)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/variables/getVariableById/{variable}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ParametroDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
