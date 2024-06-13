using Api.Gateway.Models.LogEntregables.Commands;
using Api.Gateway.Models.LogsEntregables.DTOs;
using Api.Gateway.Proxies.Mensajeria.LogEntregables.Queries;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Mensajeria.LogEntregables.Queries
{
    public interface IQLEntregableMensajeriaProxy
    {
        Task CreateHistorial(LogEntregableCreateCommand historial);
    }

    public class QLEntregableMensajeriaProxy : IQLEntregableMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QLEntregableMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
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

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/logEntregables/createHistorial", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
