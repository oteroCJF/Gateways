using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Estatus
{
    public interface IEstatusFacturaProxy
    {
        Task<List<EstatusDto>> GetAllEstatusFacturasAsync();
        Task<EstatusDto> GetEFByIdAsync(int estatus);
    }
    public class EstatusFacturaProxy : IEstatusFacturaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public EstatusFacturaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusFacturasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/facturas");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EstatusDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<EstatusDto> GetEFByIdAsync(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}estatus/facturas/getEFacturaById/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EstatusDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
