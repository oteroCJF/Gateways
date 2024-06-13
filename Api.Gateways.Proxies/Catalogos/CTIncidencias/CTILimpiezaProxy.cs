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
    public interface ICTILimpiezaProxy
    {
        Task<List<CTILimpiezaDto>> GetAllIncidenciasAsync();
        Task<List<CTILimpiezaDto>> GetIncidenciasByTipo(int incidencia);
        Task<List<CTILimpiezaDto>> GetNombresByTipo(string tipo);
        Task<CTILimpiezaDto> GetIncidenciaById(int id);
    }

    public class CTILimpiezaProxy : ICTILimpiezaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTILimpiezaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CTILimpiezaDto>> GetAllIncidenciasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/ilimpieza");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTILimpiezaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<CTILimpiezaDto>> GetIncidenciasByTipo(int incidencia)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/ilimpieza/getIncidenciasByTipo/{incidencia}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTILimpiezaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<CTILimpiezaDto>> GetNombresByTipo(string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/ilimpieza/getNombresByTipo/{tipo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTILimpiezaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<CTILimpiezaDto> GetIncidenciaById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/ilimpieza/getIncidenciaById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTILimpiezaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
