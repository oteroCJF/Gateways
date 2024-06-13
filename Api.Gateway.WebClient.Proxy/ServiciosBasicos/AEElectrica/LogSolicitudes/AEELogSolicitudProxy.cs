using Api.Gateway.Models.LogSolicitudes.Commands;
using Api.Gateway.Models.LogSolicitudes.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.ServiciosBasicos.AEElectrica.LogSolicitudes
{
    public interface IAEELogSolicitudProxy
    {
        Task<List<LogSolicitudDto>> GetHistorialBySolicitud(int cedula);
        Task<int> CreateHistorial([FromBody] LogSolicitudCreateCommand historial);
    }

    public class AEELogSolicitudProxy : IAEELogSolicitudProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public AEELogSolicitudProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<LogSolicitudDto>> GetHistorialBySolicitud(int solicitud)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/logSolicitudes/getHistorialBySolicitud/{solicitud}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LogSolicitudDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> CreateHistorial([FromBody] LogSolicitudCreateCommand historial)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(historial),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}aeelectrica/logSolicitudes/createHistorial", content);
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
