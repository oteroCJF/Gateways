using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Models.Catalogos.DTOs.Destinos;
using Api.Gateway.Proxies.Config;
using System;
using Api.Gateway.Models.Catalogos.DTOs.DiasInhabiles;

namespace Api.Gateway.Proxies.Catalogos.CTDiasInhabiles
{
    public interface ICTDiasInhabilesProxy
    {
        Task<List<DiaInhabilDto>> GetAllDiasInhabiles();
        Task<DiaInhabilDto> GetDiasInhabilesByAnio(int anio);
        Task<bool> EsDiaInhabil(int anio, string fecha);
    }
    
    public class CTDiasInhabilesProxy : ICTDiasInhabilesProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTDiasInhabilesProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<DiaInhabilDto>> GetAllDiasInhabiles()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/diasinhabiles");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<DiaInhabilDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<DiaInhabilDto> GetDiasInhabilesByAnio(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/diasinhabiles/getDiasInhabilesByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DiaInhabilDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<bool> EsDiaInhabil(int anio, string fecha)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/diasinhabiles/esdiaInhabil/{anio}/{fecha}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<bool>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
