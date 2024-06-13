using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.Repsuestas.Commands
{
    public interface ICRespuestaComedorProxy
    {
        Task UpdateRespuestas(List<RespuestasUpdateCommand> respuestas);
    }

    public class CRespuestaComedorProxy : ICRespuestaComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CRespuestaComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task UpdateRespuestas(List<RespuestasUpdateCommand> respuestas)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(respuestas),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}comedor/respuestas/updateRespuestasByCedula", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
