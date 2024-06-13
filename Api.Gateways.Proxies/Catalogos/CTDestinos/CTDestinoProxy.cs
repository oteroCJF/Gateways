using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Models.Catalogos.DTOs.Destinos;

namespace Api.Gateway.Proxies.Catalogos.CTDestinos
{
    public interface ICTDestinoProxy
    {
        Task<List<CTDestinoDto>> GetAllCTDestinos();
        Task<CTDestinoDto> GetDestinoByIdAsync(int id);
    }
    
    public class CTDestinoProxy : ICTDestinoProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTDestinoProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CTDestinoDto>> GetAllCTDestinos()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/destinos");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTDestinoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTDestinoDto> GetDestinoByIdAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/destinos/getDestinoById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTDestinoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
