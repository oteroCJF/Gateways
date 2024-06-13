using Api.Gateway.Models.Catalogos.DTOs.MarcoJuridico;
using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Catalogos.CTMarcoJuridico
{
    public interface ICTMarcoJuridicoProxy
    {
        Task<List<MarcoJuridicoDto>> GetAllMarcoJuridico();
        Task<MarcoJuridicoDto> GetMarcoJuridicoById(int servicio);        
    }

    public class CTMarcoJuridicoProxy : ICTMarcoJuridicoProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CTMarcoJuridicoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<MarcoJuridicoDto>> GetAllMarcoJuridico()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/marcoJuridico");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MarcoJuridicoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<MarcoJuridicoDto> GetMarcoJuridicoById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}catalogos/marcoJuridico/getMarcoJuridicoById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MarcoJuridicoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
