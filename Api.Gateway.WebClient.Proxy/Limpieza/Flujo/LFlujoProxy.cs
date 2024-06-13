using Api.Gateway.Models.Flujos.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Limpieza.Flujo
{
    public interface ILFlujoProxy
    {
        Task<List<FlujoDto>> GetEstatusByCedula(int estatus);

    }

    public class LFlujoProxy : ILFlujoProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public LFlujoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<FlujoDto>> GetEstatusByCedula(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/flujo/getFlujoByCedulaEstatus/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FlujoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
