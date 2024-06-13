using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Models.LogSolicitudes.Commands;
using Api.Gateway.Models.LogSolicitudes.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.LogSolicitudes
{
    public interface IAEELogSolicitudProxy
    {
        Task<List<LogSolicitudDto>> GetHistorialBySolicitud(int cedula);
        Task<int> CreateHistorial([FromBody] LogSolicitudCreateCommand historial);
    }

    public class AEELogSolicitudProxy : IAEELogSolicitudProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public AEELogSolicitudProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<LogSolicitudDto>> GetHistorialBySolicitud(int solicitud)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/logSolicitudes/getHistorialBySolicitud/{solicitud}");
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

            var request = await _httpClient.PostAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/logSolicitudes/createHistorial", content);
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
