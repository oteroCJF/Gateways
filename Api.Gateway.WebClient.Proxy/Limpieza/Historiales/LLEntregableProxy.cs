using Api.Gateway.Models.LogEntregables.Commands;
using Api.Gateway.Models.LogsEntregables.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Limpieza.Historiales
{
    public interface ILLogEntregableProxy
    {
        Task<List<LogEntregableSBDto>> GetHistorialEntregablesByCedula(int cedula);
        Task CreateHistorial(LogEntregableCreateCommand historial);
    }

    public class LLEntregableProxy : ILLogEntregableProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public LLEntregableProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<LogEntregableSBDto>> GetHistorialEntregablesByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/logEntregables/getHistorialEntregablesByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LogEntregableSBDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task CreateHistorial(LogEntregableCreateCommand historial)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(historial),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/logEntregables/createHistorial", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
