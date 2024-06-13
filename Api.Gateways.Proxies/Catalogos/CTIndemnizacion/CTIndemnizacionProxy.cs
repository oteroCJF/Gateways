using Api.Gateway.Models;
using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Models.Catalogos.DTOs.Indemnizaciones;
using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Meses.DTOs;
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

namespace Api.Gateway.Proxies.Catalogos.CTIndemnizacion
{
    public interface ICTIndemnizacionProxy
    {
        Task<List<CTIndemnizacionDto>> GetAllIndemnizacionesAsync();
        Task<List<CTIndemnizacionDto>> GetIndemnizacionByIncidencia(int incidencia);
        Task<CTIndemnizacionDto> GetIndemnizacionById(int id);
    }

    public class CTIndemnizacionProxy : ICTIndemnizacionProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTIndemnizacionProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CTIndemnizacionDto>> GetAllIndemnizacionesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/indemnizacion");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIndemnizacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTIndemnizacionDto>> GetIndemnizacionByIncidencia(int incidencia)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/indemnizacion/getIndemnizacionByIncidencia/{incidencia}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIndemnizacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTIndemnizacionDto> GetIndemnizacionById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/indemnizacion/getIndemnizacionById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTIndemnizacionDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

    }
}
