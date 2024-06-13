using Api.Gateway.Models.LogEntregables.Commands;
using Api.Gateway.Models.LogEntregables.DTOs;
using Api.Gateway.Models.LogsEntregables.Commands;
using Api.Gateway.Models.LogsEntregables.DTOs;
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

namespace Api.Gateway.Proxies.Agua.LogEntregables.Commands
{
    public interface ICLEntregableAguaProxy
    {
        Task CreateHistorial([FromBody] LogEntregableCreateCommand historial);
    }

    public class CLEntregableAguaProxy : ICLEntregableAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CLEntregableAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task CreateHistorial([FromBody] LogEntregableCreateCommand historial)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(historial),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/logEntregables/createHistorial", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
