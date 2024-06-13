using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTServicios
{
    public interface ICTServicioProxy
    {
        Task<List<CTServicioDto>> GetAllCatalogoServiciosAsync();
        Task<CTServicioDto> GetServicioByIdAsync(int servicio);
    }

    public class CTServicioProxy : ICTServicioProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTServicioProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTServicioDto>> GetAllCatalogoServiciosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/servicios");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTServicioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTServicioDto> GetServicioByIdAsync(int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/servicios/getServicioById/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTServicioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
