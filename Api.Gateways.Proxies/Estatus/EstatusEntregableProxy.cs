using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EntregablesEstatus;
using Api.Gateway.Models.Estatus.DTOs.EstatusCedulas;
using Api.Gateway.Models.Estatus.DTOs.EstatusEntregables;
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
    public interface IEstatusEntregableProxy
    {
        Task<List<EstatusDto>> GetAllEstatusEntregablesAsync();
        Task<EstatusDto> GetEEByIdAsync(int estatus);
        Task<List<EntregableEstatusDto>> GetEntregablesByEstatus(int servicio, int estatus, string flujo);
        Task<List<FlujoEntregableDto>> GetFlujoByEntregableServicio(int servicio);
        Task<List<FlujoEntregableDto>> GetFlujoByEntregablesSE(int servicio, int estatusC);
        Task<EEntregableCedulaDto> GetEEntregableByEC(int estatus, int entregable, string flujo);
        Task<List<EEntregableCedulaDto>> GetEEntregablesByEC(int estatus, string flujo);
    }

    public class EstatusEntregableProxy : IEstatusEntregableProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public EstatusEntregableProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<EstatusDto>> GetAllEstatusEntregablesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/entregables");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EstatusDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<EstatusDto> GetEEByIdAsync(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/entregables/getEEntregableById/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EstatusDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<EntregableEstatusDto>> GetEntregablesByEstatus(int servicio, int estatus, string flujo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/entregables/getEntregablesByEstatus/{servicio}/{estatus}/{flujo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EntregableEstatusDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<FlujoEntregableDto>> GetFlujoByEntregableServicio(int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/flujos/entregable/getAllFlujoByEntregablesServicio/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FlujoEntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<FlujoEntregableDto>> GetFlujoByEntregablesSE(int servicio, int estatusC)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/flujos/entregable/getFlujoByEntregablesSE/{servicio}/{estatusC}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FlujoEntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<EEntregableCedulaDto> GetEEntregableByEC(int estatus, int entregable, string flujo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/entregables/getEEntregableByEC/{estatus}/{entregable}/{flujo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EEntregableCedulaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<EEntregableCedulaDto>> GetEEntregablesByEC(int estatus, string flujo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.EstatusUrl}api/estatus/entregables/getEEntregablesByEC/{estatus}/{flujo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EEntregableCedulaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
