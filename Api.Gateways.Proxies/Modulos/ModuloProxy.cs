using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Modulos.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Modulos
{
    public interface IModuloProxy
    {
        Task<List<ModuloDto>> GetAllModulosAsync();
        Task<ModuloDto> GetModuloByIdAsync(int modulo);
    }
    public class ModuloProxy : IModuloProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public ModuloProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<ModuloDto>> GetAllModulosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ModulosUrl}api/modulos");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ModulosUrl}api/modulos/{modulo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ModuloDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
