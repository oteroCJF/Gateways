using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusEntregables;
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
    public interface IEstatusEntregableProxy
    {
        Task<List<EstatusDto>> GetAllEstatusEntregablesAsync();
        Task<EstatusDto> GetEEByIdAsync(int estatus);
        Task<List<FlujoEntregableDto>> GetFlujoByEntregableServicio(int servicio);
        Task<List<FlujoEntregableDto>> GetFlujoByEntregablesSE(int servicio, int estatusC);
    }
    public class EstatusEntregableProxy : IEstatusEntregableProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public EstatusEntregableProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusEntregablesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/entregables");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EstatusDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<EstatusDto> GetEEByIdAsync(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/entregables/getEEntregableById/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EstatusDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<FlujoEntregableDto>> GetFlujoByEntregableServicio(int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/entregables/getAllFlujoByEntregablesServicio/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FlujoEntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<FlujoEntregableDto>> GetFlujoByEntregablesSE(int servicio, int estatusC)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/entregables/GetFlujoByEntregablesSE/{servicio}/{estatusC}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FlujoEntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
