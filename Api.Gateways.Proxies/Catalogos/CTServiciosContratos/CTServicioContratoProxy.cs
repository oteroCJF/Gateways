using Api.Gateway.Models;
using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Catalogos.DTOs.ServiciosContratos;
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

namespace Api.Gateway.Proxies.Catalogos.CTServiciosContratos
{
    public interface ICTServicioContratoProxy
    {
        Task<List<CTServicioContratoDto>> GetAllServiciosContratosAsync();
        Task<List<CTServicioContratoDto>> GetServiciosByServicioAsync(int servicio);
        Task<CTServicioContratoDto> GetServicioContratoByIdAsync(int servicio);
    }

    public class CTServicioContratoProxy : ICTServicioContratoProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTServicioContratoProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CTServicioContratoDto>> GetAllServiciosContratosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/serviciosContrato");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTServicioContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTServicioContratoDto>> GetServiciosByServicioAsync(int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/serviciosContrato/getServiciosByServicio/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTServicioContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTServicioContratoDto> GetServicioContratoByIdAsync(int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/serviciosContrato/getServicioContratoById/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTServicioContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
