using Api.Gateway.Models;
using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
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

namespace Api.Gateway.Proxies.Catalogos.CTIncidencias
{
    public interface ICTIAguaProxy
    {
        Task<List<CTIAguaDto>> GetAllIncidenciasAsync();
        Task<List<CTIAguaDto>> GetIncidenciasByTipo(int incidencia);
        Task<CTIAguaDto> GetIncidenciaById(int id);
    }

    public class CTIAguaProxy : ICTIAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTIAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CTIAguaDto>> GetAllIncidenciasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/iagua");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIAguaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<CTIAguaDto>> GetIncidenciasByTipo(int incidencia)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/iagua/getIncidenciasByTipo/{incidencia}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIAguaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<CTIAguaDto> GetIncidenciaById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/iagua/getIncidenciaById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTIAguaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
