using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Mensajeria;
using Api.Gateway.Models;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;

namespace Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion
{
    public interface IQCedulaMensajeriaProxy
    {
        Task<List<CedulaMensajeriaDto>> GetAllCedulasAsync();
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes, int contrato);
        Task<CedulaMensajeriaDto> GetCedulaById(int cedula);
        Task<decimal> GetTotalPDAsync(int cedula);
    }

    public class QCedulaMensajeriaProxy : IQCedulaMensajeriaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QCedulaMensajeriaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CedulaMensajeriaDto>> GetAllCedulasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/cedulaEvaluacion");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaMensajeriaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/cedulaEvaluacion/getCedulasByAnio/{anio}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/cedulaEvaluacion/getCedulasByAnioMes/{anio}/{mes}/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DataCollection<CedulaEvaluacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<CedulaMensajeriaDto> GetCedulaById(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/cedulaEvaluacion/getCedulaById/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaMensajeriaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/cedulaEvaluacion/getTotalPD/{cedula}");
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
