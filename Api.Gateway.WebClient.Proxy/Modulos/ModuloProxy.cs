using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Modulos.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Modulos
{
    public interface IModuloProxy
    {
        Task<List<ModuloDto>> GetAllModulosAsync();
        Task<ModuloDto> GetModuloByIdAsync(int modulo);
        Task<SubmoduloDto> GetSubmoduloByIdAsync(int submodulo);
    }

    public class ModuloProxy : IModuloProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public ModuloProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<ModuloDto>> GetAllModulosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}modulos");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ModuloDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<ModuloDto> GetModuloByIdAsync(int modulo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}modulos/{modulo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ModuloDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<SubmoduloDto> GetSubmoduloByIdAsync(int submodulo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}modulos/getSubmoduloById/{submodulo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<SubmoduloDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
