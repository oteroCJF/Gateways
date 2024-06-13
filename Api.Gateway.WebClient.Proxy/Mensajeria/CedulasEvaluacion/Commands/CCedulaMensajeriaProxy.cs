using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Mensajeria;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Mensajeria.CedulasEvaluacion.Commands
{
    public interface ICCedulaMensajeriaProxy
    {
        Task<CedulaMensajeriaDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula);
    }

    public class CCedulaMensajeriaProxy : ICCedulaMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CCedulaMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<CedulaMensajeriaDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(cedula),
                   Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}mensajeria/cedulaEvaluacion/updateCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaMensajeriaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

    }
}
