using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusOficios;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Estatus
{
    public interface IEstatusOficioProxy 
    { 
        Task<List<EstatusDto>> GetAllEstatusOficiosAsync();
        Task<EstatusDto> GetEstatusOficioByIdAsync(int estatus);
        Task<List<FlujoOficiosDto>> GetFlujoByOficio(int servicio, int estatusO);
    }

    public class EstatusOficioProxy : IEstatusOficioProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public EstatusOficioProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusOficiosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/oficios");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EstatusDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<EstatusDto> GetEstatusOficioByIdAsync(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/oficios/getEFacturaById/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EstatusDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<FlujoOficiosDto>> GetFlujoByOficio(int servicio, int estatusO)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/oficios/getFlujoByOficio/{servicio}/{estatusO}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FlujoOficiosDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
