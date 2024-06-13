using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Proxies.Comedor.LogCedula.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.LogCedulas.Commands
{
    public interface ICLCedulaComedorProxy
    {
        Task CreateHistorial(LogCedulaCreateCommand historial);
    }

    public class CLCedulaComedorProxy : ICLCedulaComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CLCedulaComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
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

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}comedor/logCedulas/createHistorial", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
