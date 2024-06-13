using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Proxies.Agua.LogCedulas.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Agua.LogCedulas.Commands
{
    public interface ICLCedulaAguaProxy
    {
        Task CreateHistorial(LogCedulaCreateCommand historial);
    }

    public class CLCedulaAguaProxy : ICLCedulaAguaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CLCedulaAguaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
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

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}agua/logCedulas/createHistorial", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
