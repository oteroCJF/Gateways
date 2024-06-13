using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Mensajeria;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Proxies.Mensajeria.Respuestas.Commands;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Mensajeria.Respuestas.Queries
{
    public interface IQRespuestaMensajeriaProxy
    {
        Task<List<MRespuestaDto>> GetAllRespuestasByAnioAsync(int anio);
        Task<bool> GetDeductivasByCedula(int cedula, int anio);
        Task<List<MRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula);
    }

    public class QRespuestaMensajeriaProxy : IQRespuestaMensajeriaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QRespuestaMensajeriaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<MRespuestaDto>> GetAllRespuestasByAnioAsync(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/respuestasEvaluacion/getRespuestasByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MRespuestaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<bool> GetDeductivasByCedula(int cedula, int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/respuestasEvaluacion/getDeductivasByCedula/{cedula}/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<bool>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<MRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/respuestasEvaluacion/getRespuestasByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MRespuestaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
