using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Mensajeria;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Transporte.Respuestas.Queries
{
    public interface IQRespuestaTransporteProxy
    {
        Task<List<TRespuestaDto>> GetAllRespuestasByAnioAsync(int anio);
        Task<bool> GetDeductivasByCedula(int cedula, int anio);
        Task<List<TRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula);
    }

    public class QRespuestaTransporteProxy : IQRespuestaTransporteProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QRespuestaTransporteProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<TRespuestaDto>> GetAllRespuestasByAnioAsync(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/respuestasEvaluacion/getRespuestasByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<TRespuestaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<bool> GetDeductivasByCedula(int cedula, int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/respuestasEvaluacion/getDeductivasByCedula/{cedula}/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<bool>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<TRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/respuestasEvaluacion/getRespuestasByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<TRespuestaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
