using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Models.Repositorios.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Fumigacion.Repositorios
{
    public interface IFRepositorioProxy
    {
        Task<List<RepositorioDto>> GetAllRepositorios(int anio);
        Task<RepositorioDto> GetRepositorioById(int repositorio);
        Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand command);
    }
    public class FRepositorioProxy : IFRepositorioProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public FRepositorioProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<RepositorioDto>> GetAllRepositorios(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/repositorios/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<RepositorioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<RepositorioDto> GetRepositorioById(int repositorio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/repositorios/getRepositorioById/{repositorio}");
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

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}fumigacion/repositorios/createRepositorio", content);
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
