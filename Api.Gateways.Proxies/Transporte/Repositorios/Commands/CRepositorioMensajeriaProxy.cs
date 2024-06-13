using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Models.Repositorios.DTOs;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Api.Gateway.Proxies.Config;

namespace Api.Gateway.Proxies.Transporte.Repositorios.Commands
{
    public interface ICRepositorioTransporteProxy
    {
        Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand repositorio);
    }

    public class CRepositorioTransporteProxy : ICRepositorioTransporteProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CRepositorioTransporteProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand repositorio)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(repositorio),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.TransporteUrl}api/transporte/repositorio/createRepositorio", content);
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
