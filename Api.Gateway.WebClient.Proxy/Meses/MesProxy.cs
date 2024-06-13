using Api.Gateway.Models;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Meses
{
    public interface IMesProxy
    {
        Task<List<MesDto>> GetAllAsync();
        Task<MesDto> GetAsync(int id);
    }
    
    public class MesProxy: IMesProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public MesProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<MesDto>> GetAllAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}meses");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MesDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<MesDto> GetAsync(int mes)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}meses/getMes/" + mes);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MesDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
