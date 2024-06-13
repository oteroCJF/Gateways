using Api.Gateway.Models.Firmantes.Commands;
using Api.Gateway.Models.Firmantes.DTOs;
using Api.Gateway.Proxies.Comedor.Firmantes.Commands;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Comedor.Firmantes.Queries
{
    public interface IQFirmanteComedorProxy
    {
        Task<List<FirmanteDto>> GetAllFirmantesAsync();
        Task<FirmanteDto> GetFirmanteById(int firmante);
        Task<List<FirmanteDto>> GetFirmantesByInmueble(int inmueble);
    }

    public class QFirmanteComedorProxy : IQFirmanteComedorProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QFirmanteComedorProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<FirmanteDto>> GetAllFirmantesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/firmantes");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FirmanteDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<FirmanteDto> GetFirmanteById(int firmante)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/firmantes/getFirmanteById/{firmante}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<FirmanteDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<FirmanteDto>> GetFirmantesByInmueble(int inmueble)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/firmantes/getFirmantesByInmueble/{inmueble}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FirmanteDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}



