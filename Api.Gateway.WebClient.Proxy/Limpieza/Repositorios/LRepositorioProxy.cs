using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Models.Repositorios.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Limpieza.Repositorios
{
    public interface ILRepositorioProxy
    {
        Task<List<RepositorioDto>> GetAllRepositorios(int anio);
        Task<RepositorioDto> GetRepositorioById(int facturacion);
        Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand command);
    }

    public class LRepositorioProxy : ILRepositorioProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public LRepositorioProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<RepositorioDto>> GetAllRepositorios(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/repositorios/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<RepositorioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<RepositorioDto> GetRepositorioById(int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/repositorios/getRepositorioById/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<RepositorioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand facturacion)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(facturacion),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/repositorios/createRepositorio", content);
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
