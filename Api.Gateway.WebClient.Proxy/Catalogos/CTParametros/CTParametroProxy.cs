using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTParametros
{
    public interface ICTParametroProxy
    {
        Task<List<CTParametroDto>> GetAllParametrosAsync();
        Task<CTParametroDto> GetServicioByIdAsync(int servicio);
        Task<List<CTParametroDto>> GetParametroByTipo(string tipo);
        Task<List<CTParametroDto>> GetParametroByTabla(string tabla);
    }

    public class CTParametroProxy : ICTParametroProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTParametroProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTParametroDto>> GetAllParametrosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/parametros");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTParametroDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTParametroDto> GetServicioByIdAsync(int parametro)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/parametros/getParametroById/{parametro}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTParametroDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTParametroDto>> GetParametroByTipo(string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/parametros/getParametroByTipo/{tipo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTParametroDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTParametroDto>> GetParametroByTabla(string tabla)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/parametros/getParametroByTabla/{tabla}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTParametroDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
