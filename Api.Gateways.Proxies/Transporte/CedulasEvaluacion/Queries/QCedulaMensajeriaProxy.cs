using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Transporte;
using Api.Gateway.Models;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;

namespace Api.Gateway.Proxies.Transporte.CedulasEvaluacion
{
    public interface IQCedulaTransporteProxy
    {
        Task<List<CedulaTransporteDto>> GetAllCedulasAsync();
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes, int contrato);
        Task<CedulaTransporteDto> GetCedulaById(int cedula);
        Task<decimal> GetTotalPDAsync(int cedula);
    }

    public class QCedulaTransporteProxy : IQCedulaTransporteProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QCedulaTransporteProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CedulaTransporteDto>> GetAllCedulasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/cedulaEvaluacion");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaTransporteDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/cedulaEvaluacion/getCedulasByAnio/{anio}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/cedulaEvaluacion/getCedulasByAnioMes/{anio}/{mes}/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DataCollection<CedulaEvaluacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<CedulaTransporteDto> GetCedulaById(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/cedulaEvaluacion/getCedulaById/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaTransporteDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/cedulaEvaluacion/getTotalPD/{cedula}");
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
