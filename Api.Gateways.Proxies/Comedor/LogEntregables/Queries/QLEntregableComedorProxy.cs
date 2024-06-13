using Api.Gateway.Models.LogEntregables.Commands;
using Api.Gateway.Models.LogEntregables.DTOs;
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

namespace Api.Gateway.Proxies.Comedor.LogEntregables.Queries
{
    public interface IQLEntregableComedorProxy
    {
        Task<List<LogEntregableDto>> GetHistorialEntregablesByCedula(int cedula);
    }

    public class QLEntregableComedorProxy : IQLEntregableComedorProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QLEntregableComedorProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<LogEntregableDto>> GetHistorialEntregablesByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/logEntregables/getHistorialEntregablesByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LogEntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}


