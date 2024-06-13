using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Agua;
using Api.Gateway.Models;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;

namespace Api.Gateway.Proxies.Agua.CedulasEvaluacion
{
    public interface IQCedulaAguaProxy
    {
        Task<List<CedulaAguaDto>> GetAllCedulasAsync();
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes, int contrato);
        Task<CedulaAguaDto> GetCedulaById(int cedula);
        Task<decimal> GetTotalPDAsync(int cedula);
    }

    public class QCedulaAguaProxy : IQCedulaAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QCedulaAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CedulaAguaDto>> GetAllCedulasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/cedulaEvaluacion");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaAguaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/cedulaEvaluacion/getCedulasByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DataCollection<CedulaEvaluacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes, int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/cedulaEvaluacion/getCedulasByAnioMes/{anio}/{mes}/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DataCollection<CedulaEvaluacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<CedulaAguaDto> GetCedulaById(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/cedulaEvaluacion/getCedulaById/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaAguaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/cedulaEvaluacion/getTotalPD/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<decimal>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }                
    }
}
