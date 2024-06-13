using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Comedor;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.Repsuestas.Queries
{
    public interface IQRespuestaComedorProxy
    {
        Task<List<CRespuestaDto>> GetAllRespuestasByAnioAsync(int anio);
        Task<List<CRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula);
        Task<bool> VerificaDeductivas(int cedulaId);
    }

    public class QRespuestaComedorProxy : IQRespuestaComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QRespuestaComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CRespuestaDto>> GetAllRespuestasByAnioAsync(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/respuestasEvaluacion/getRespuestasByAnio/{anio}");
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
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/respuestasEvaluacion/{cedula}");
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
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/respuestasEvaluacion/verificaDeductivas/{cedulaId}");
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

