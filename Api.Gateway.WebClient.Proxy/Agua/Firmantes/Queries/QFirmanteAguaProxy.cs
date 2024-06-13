using Api.Gateway.Models.Firmantes.Commands;
using Api.Gateway.Models.Firmantes.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Agua.Firmantes.Queries
{
    public interface IQFirmanteAguaProxy
    {
        Task<List<FirmanteDto>> GetAllFirmantesAsync();
        Task<FirmanteDto> GetFirmanteById(int firmante);
        Task<List<FirmanteDto>> GetFirmantesByInmueble(int inmueble);
    }

    public class QFirmanteAguaProxy : IQFirmanteAguaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QFirmanteAguaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<FirmanteDto>> GetAllFirmantesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/firmantes");
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
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/firmantes/getFirmanteById/{firmante}");
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
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/firmantes/getFirmantesByInmueble/{inmueble}");
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
