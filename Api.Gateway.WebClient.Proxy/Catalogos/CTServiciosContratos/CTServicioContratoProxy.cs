using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Catalogos.DTOs.ServiciosContratos;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTServiciosContratos
{
    public interface ICTServicioContratoProxy
    {
        Task<List<CTServicioContratoDto>> GetAllServiciosContratosAsync();
        Task<List<CTServicioContratoDto>> GetServiciosByServicioAsync(int servicio);
    }

    public class CTServicioContratoProxy : ICTServicioContratoProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTServicioContratoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTServicioContratoDto>> GetAllServiciosContratosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/serviciosContrato");
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
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/serviciosContrato/getServiciosByServicio/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTServicioContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
