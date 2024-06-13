using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Comedor;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Comedor.Respuestas.Queries
{
    public interface IQRespuestaComedorProxy
    {
        Task<List<CRespuestaDto>> GetAllRespuestasByAnioAsync(int anio);
        Task<List<CRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula);
        Task<bool> VerificaDeductivas(int cedulaId);
    }

    public class QRespuestaComedorProxy : IQRespuestaComedorProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QRespuestaComedorProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CRespuestaDto>> GetAllRespuestasByAnioAsync(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/respuestasEvaluacion/getRespuestasByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CRespuestaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/respuestasEvaluacion/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CRespuestaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<bool> VerificaDeductivas(int cedulaId)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/respuestasEvaluacion/verificaDeductivas/{cedulaId}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<bool>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}

