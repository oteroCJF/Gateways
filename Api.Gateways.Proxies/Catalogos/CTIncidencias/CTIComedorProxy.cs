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
    public interface ICTIComedorProxy
    {
        Task<List<CTIComedorDto>> GetAllIncidenciasAsync();
        Task<List<CTIComedorDto>> GetIncidenciasByTipo(int incidencia);
        Task<List<CTIComedorDto>> GetNombresByTipo(int tipo);
        Task<CTIComedorDto> GetIncidenciaById(int id);
    }

    public class CTIComedorProxy : ICTIComedorProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTIComedorProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CTIComedorDto>> GetAllIncidenciasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/icomedor");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIComedorDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<CTIComedorDto>> GetIncidenciasByTipo(int incidencia)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/icomedor/getIncidenciasByTipo/{incidencia}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIComedorDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<CTIComedorDto>> GetNombresByTipo(int tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/icomedor/getNombresByTipo/{tipo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTIComedorDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<CTIComedorDto> GetIncidenciaById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/icomedor/getIncidenciaById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTIComedorDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
