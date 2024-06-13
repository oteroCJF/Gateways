using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Models.Catalogos.DTOs.ActividadesContrato;

namespace Api.Gateway.Proxies.Catalogos.CTActividadesContrato
{
    public interface ICTActividadContratoProxy
    {
        Task<List<CTActividadContratoDto>> GetAllCTActividades();
        Task<CTActividadContratoDto> GetActividadByIdAsync(int id);
    }
    
    public class CTActividadContratoProxy : ICTActividadContratoProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTActividadContratoProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CTActividadContratoDto>> GetAllCTActividades()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/actividades");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTActividadContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTActividadContratoDto> GetActividadByIdAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/actividades/getPartidabmById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTActividadContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
