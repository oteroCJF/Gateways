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

namespace Api.Gateway.Proxies.Fumigacion.Facturacion
{
    public interface IFRepositorioProxy
    {
        Task<List<RepositorioDto>> GetAllFacturacionesAsync(int anio);
        Task<RepositorioDto> GetFacturacionByIdAsync(int repositorio);
        Task<int> CreateFacturacion([FromBody] RepositorioCreateCommand repositorio);
    }

    public class FRepositorioProxy : IFRepositorioProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public FRepositorioProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<RepositorioDto>> GetAllFacturacionesAsync(int anio)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/repositorios/" + anio);
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<RepositorioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch(HttpRequestException e)
            {
                return new List<RepositorioDto>();
            }
        }

        public async Task<RepositorioDto> GetFacturacionByIdAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/repositorios/getRepositorioById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<RepositorioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> CreateFacturacion([FromBody] RepositorioCreateCommand repositorio)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(repositorio),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/repositorios/createRepositorio", content);
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

