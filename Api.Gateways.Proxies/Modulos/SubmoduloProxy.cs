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
    public interface ISubmoduloProxy
    {
        Task<List<SubmoduloDto>> GetSubmoduloByModuloAsync(int modulo);
        Task<SubmoduloDto> GetSubmoduloByIdAsync(int submodulo);
    }
    public class SubmoduloProxy : ISubmoduloProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public SubmoduloProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<SubmoduloDto>> GetSubmoduloByModuloAsync(int modulo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ModulosUrl}api/modulos/submodulos/{modulo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<SubmoduloDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<SubmoduloDto> GetSubmoduloByIdAsync(int submodulo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ModulosUrl}api/modulos/submodulos/getSubmoduloById/{submodulo}");
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
