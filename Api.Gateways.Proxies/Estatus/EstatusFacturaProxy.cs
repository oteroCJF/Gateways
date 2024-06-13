using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Estatus
{
    public interface IEstatusFacturaProxy
    {
        Task<List<EstatusDto>> GetAllEstatusFacturasAsync();
        Task<EstatusDto> GetEFByIdAsync(int estatus);
    }

    public class EstatusFacturaProxy : IEstatusFacturaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public EstatusFacturaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusFacturasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/facturas");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/facturas/getEFacturaById/{estatus}");
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
