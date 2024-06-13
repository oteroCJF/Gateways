using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Models.Repositorios.DTOs;
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

namespace Api.Gateway.Proxies.Limpieza.Repositorios
{
    public interface ILRepositorioProxy
    {
        Task<List<RepositorioDto>> GetAllRepositoriosAsync(int anio);
        Task<RepositorioDto> GetRepositorioByIdAsync(int repositorio);
        Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand repositorio);
    }

    public class LRepositorioProxy : ILRepositorioProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public LRepositorioProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<RepositorioDto>> GetAllRepositoriosAsync(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/repositorios/" + anio);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<RepositorioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<RepositorioDto> GetRepositorioByIdAsync(int repositorio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/repositorios/getRepositorioById/{repositorio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<RepositorioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand repositorio)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(repositorio),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/repositorios/createRepositorio", content);
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
