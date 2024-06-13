using Api.Gateway.Models.LogEntregables.Commands;
using Api.Gateway.Models.LogsEntregables.DTOs;
using Api.Gateway.Proxies.Agua.LogEntregables.Queries;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Agua.LogEntregables.Queries
{
    public interface IQLEntregableAguaProxy
    {
        Task CreateHistorial(LogEntregableCreateCommand historial);
    }

    public class QLEntregableAguaProxy : IQLEntregableAguaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QLEntregableAguaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task CreateHistorial(LogEntregableCreateCommand historial)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(historial),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}agua/logEntregables/createHistorial", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
