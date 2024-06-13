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

namespace Api.Gateway.Proxies.Comedor.Repositorios.Queries
{
    public interface IQRepositorioComedorProxy
    {
        Task<List<RepositorioDto>> GetAllRepositoriosAsync(int anio);
        Task<RepositorioDto> GetRepositorioByAMC(int anio, int mes, int contrato);
        Task<RepositorioDto> GetRepositorioByIdAsync(int repositorio);
    }

    public class QRepositorioComedorProxy : IQRepositorioComedorProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QRepositorioComedorProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<RepositorioDto>> GetAllRepositoriosAsync(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/repositorios/" + anio);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<RepositorioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<RepositorioDto> GetRepositorioByAMC(int anio, int mes, int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/repositorios/getRepositorioByAMC/{anio}/{mes}/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<RepositorioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<RepositorioDto> GetRepositorioByIdAsync(int repositorio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/repositorios/getRepositorioById/{repositorio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<RepositorioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
