using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Limpieza;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Mensajeria;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Limpieza.CedulaEvaluacion
{
    public interface ILRespuestaProxy
    {
        Task<List<LRespuestaDto>> GetAllRespuestasByAnioAsync(int anio);
        Task<List<LRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula);
        Task UpdateRespuestas(List<RespuestasUpdateCommand> respuestas);
    }

    public class LRespuestaProxy : ILRespuestaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public LRespuestaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<LRespuestaDto>> GetAllRespuestasByAnioAsync(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/respuestasEvaluacion/getRespuestasByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LRespuestaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<LRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/respuestasEvaluacion/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LRespuestaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task UpdateRespuestas(List<RespuestasUpdateCommand> respuestas)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(respuestas),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/respuestasEvaluacion/updateRespuestasByCedula", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
