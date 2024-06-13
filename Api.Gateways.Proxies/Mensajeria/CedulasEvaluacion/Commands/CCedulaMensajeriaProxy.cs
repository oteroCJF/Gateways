using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Mensajeria;

namespace Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion.Commands
{
    public interface ICCedulaMensajeriaProxy
    {
        Task<CedulaMensajeriaDto> EnviarCedula([FromBody] EnviarCedulaEvaluacionUpdateCommand cedula);
        Task<CedulaMensajeriaDto> DBloquearCedula([FromBody] DBloquearCedulaUpdateCommand cedula);
        Task<CedulaMensajeriaDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula);
    }

    public class CCedulaMensajeriaProxy : ICCedulaMensajeriaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CCedulaMensajeriaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<CedulaMensajeriaDto> EnviarCedula([FromBody] EnviarCedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(cedula),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PutAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/cedulaEvaluacion/enviarCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaMensajeriaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaMensajeriaDto> DBloquearCedula([FromBody] DBloquearCedulaUpdateCommand cedula)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(cedula),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PutAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/cedulaEvaluacion/dbloquearCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaMensajeriaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaMensajeriaDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(cedula),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PutAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/cedulaEvaluacion/updateCedula", content);
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
