using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Comedor;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.CedulasEvaluacion.Commands
{
    public interface ICCedulaComedorProxy
    {
        Task<CedulaComedorDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula);
    }

    public class CCedulaComedorProxy : ICCedulaComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CCedulaComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<CedulaComedorDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(cedula),
                   Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}comedor/cedulaEvaluacion/updateCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaComedorDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

    }
}
