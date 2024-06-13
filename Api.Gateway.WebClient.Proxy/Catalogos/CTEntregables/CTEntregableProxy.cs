using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Models.Entregables.ServiciosBasicos.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTEntregables
{
    public interface ICTEntregableProxy
    {
        Task<List<CTEntregableDto>> GetAllEntregablesAsync();
        Task<List<CTEntregableDto>> GetEntregablesByServicioAsync(int servicio);
        Task<CTEntregableDto> GetEntregableByIdAsync(int entregable);
    }

    public class CTEntregableProxy : ICTEntregableProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTEntregableProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTEntregableDto>> GetAllEntregablesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/entregables");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTEntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<CTEntregableDto>> GetEntregablesByServicioAsync(int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/entregables/getEntregablesByServicio/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTEntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTEntregableDto> GetEntregableByIdAsync(int entregable)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/entregables/getEntregableById/{entregable}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTEntregableDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
