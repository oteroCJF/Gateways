using Api.Gateway.Models;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Meses
{
    public interface IMesProxy
    {
        Task<List<MesDto>> GetAllMesesAsync();
        Task<MesDto> GetMesByIdAsync(int id);
    }

    public class MesProxy : IMesProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public MesProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<MesDto>> GetAllMesesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MesesUrl}api/meses");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MesDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<MesDto> GetMesByIdAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MesesUrl}api/meses/{id}");
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
