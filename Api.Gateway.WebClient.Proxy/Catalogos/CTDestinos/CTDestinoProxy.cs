using Api.Gateway.Models.Catalogos.DTOs.ActividadesContrato;
using Api.Gateway.Models.Catalogos.DTOs.Destinos;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTDestino
{
    public interface ICTDestinoProxy
    {
        Task<List<CTDestinoDto>> GetAllDestinos();
        Task<CTDestinoDto> GetDestinoByIdAsync(int id);
    }
    
    public class CTDestinoProxy : ICTDestinoProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTDestinoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTDestinoDto>> GetAllDestinos()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/destinos");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTDestinoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTDestinoDto> GetDestinoByIdAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/destinos/getActividadById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTDestinoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
