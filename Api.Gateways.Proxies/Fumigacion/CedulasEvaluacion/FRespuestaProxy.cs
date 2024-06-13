using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Fumigacion;
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

namespace Api.Gateway.Proxies.Fumigacion.CedulaEvaluacion
{
    public interface IFRespuestaProxy
    {
        Task<List<FRespuestaDto>> GetAllRespuestasByAnioAsync(int anio);
        Task<List<FRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula);
        Task UpdateRespuestas(List<RespuestasUpdateCommand> respuestas);
    }

    public class FRespuestaProxy : IFRespuestaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public FRespuestaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<FRespuestaDto>> GetAllRespuestasByAnioAsync(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/respuestasEvaluacion/getRespuestasByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FRespuestaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<FRespuestaDto>> GetRespuestasEvaluacionByCedulaAnioMes(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/respuestasEvaluacion/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FRespuestaDto>>(
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

            var request = await _httpClient.PutAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/respuestasEvaluacion/updateRespuestasByCedula", content);
            request.EnsureSuccessStatusCode();
        }
    }
}

