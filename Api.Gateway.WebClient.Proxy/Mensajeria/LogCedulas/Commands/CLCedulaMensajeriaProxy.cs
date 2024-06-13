using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Proxies.Mensajeria.LogCedulas.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Mensajeria.LogCedulas.Commands
{
    public interface ICLCedulaMensajeriaProxy
    {
        Task CreateHistorial(LogCedulaCreateCommand historial);
    }

    public class CLCedulaMensajeriaProxy : ICLCedulaMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CLCedulaMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task CreateHistorial(LogCedulaCreateCommand historial)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(historial),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/logCedulas/createHistorial", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
