using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
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

namespace Api.Gateway.Proxies.Agua.LogCedulas.Commands
{
    public interface ICLCedulaAguaProxy
    {
        Task CreateHistorial([FromBody] LogCedulaCreateCommand historial);
    }

    public class CLCedulaAguaProxy : ICLCedulaAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CLCedulaAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task CreateHistorial([FromBody] LogCedulaCreateCommand historial)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(historial),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/logCedulas/createHistorial", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
