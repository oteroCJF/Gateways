using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Agua;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Agua.Respuestas.Commands
{
    public interface ICRespuestaAguaProxy
    {
        Task UpdateRespuestas(List<RespuestasUpdateCommand> respuestas);
    }

    public class CRespuestaAguaProxy : ICRespuestaAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CRespuestaAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task UpdateRespuestas(List<RespuestasUpdateCommand> respuestas)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(respuestas),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiUrls.AguaUrl}api/agua/respuestasEvaluacion/updateRespuestasByCedula", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
