using Api.Gateway.Models.LogsEntregables.Commands;
using Api.Gateway.Models.LogsEntregables.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.ServiciosBasicos.AEElectrica.LogEntregables
{
    public interface IAEELogEntregableProxy
    {
        Task<List<LogEntregableSBDto>> GetHistorialEBySolicitud(int cedula);
        Task<int> CreateHistorialE([FromBody] LogSBEntregableCreateCommand historial);
    }

    public class AEELogEntregableProxy : IAEELogEntregableProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public AEELogEntregableProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<LogEntregableSBDto>> GetHistorialEBySolicitud(int solicitud)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/logEntregables/getHistorialEBySolicitud/{solicitud}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LogEntregableSBDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> CreateHistorialE([FromBody] LogSBEntregableCreateCommand historial)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(historial),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}aeelectrica/logEntregables/createHistorialE", content);
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
