using Api.Gateway.Models.Catalogos.DTOs.ActividadesContrato;
using Api.Gateway.Models.Catalogos.DTOs.Destinos;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTDIasInhabiles
{
    public interface ICTDiasInhabilesProxy
    {
        Task<List<CTDestinoDto>> GetAllDiasInhabiles();
        Task<CTDestinoDto> GetDiasInhabilesByAnio(int id);
        Task<bool> EsDiaInhabil(int anio, string fecha);
    }
    
    public class CTDiasInhabilesProxy : ICTDiasInhabilesProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTDiasInhabilesProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CTDestinoDto>> GetAllDiasInhabiles()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/diasInhabiles");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTDestinoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTDestinoDto> GetDiasInhabilesByAnio(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/diasInhabiles/getDiasInhabilesByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTDestinoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<bool> EsDiaInhabil(int anio, string fecha)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/diasinhabiles/esdiaInhabil/{anio}/{fecha}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<bool>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }
    }
}
