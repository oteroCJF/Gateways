using Api.Gateway.Models.Catalogos.DTOs.MarcoJuridico;
using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Catalogos.CTMarcoJuridico
{
    public interface ICTMarcoJuridicoProxy
    {
        Task<List<MarcoJuridicoDto>> GetAllMarcoJuridico();
        Task<MarcoJuridicoDto> GetMarcoJuridicoById(int id);
    }

    public class CTMarcoJuridicoProxy : ICTMarcoJuridicoProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTMarcoJuridicoProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<MarcoJuridicoDto>> GetAllMarcoJuridico()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/marcoJuridico");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MarcoJuridicoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<MarcoJuridicoDto> GetMarcoJuridicoById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/marcoJuridico/getMarcoJuridicoById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MarcoJuridicoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
