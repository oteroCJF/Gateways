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
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Transporte;

namespace Api.Gateway.Proxies.Transporte.CedulasEvaluacion.Commands
{
    public interface ICCedulaTransporteProxy
    {
        Task<CedulaTransporteDto> EnviarCedula([FromBody] EnviarCedulaEvaluacionUpdateCommand cedula);
        Task<CedulaTransporteDto> DBloquearCedula([FromBody] DBloquearCedulaUpdateCommand cedula);
        Task<CedulaTransporteDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula);
    }

    public class CCedulaTransporteProxy : ICCedulaTransporteProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CCedulaTransporteProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<CedulaTransporteDto> EnviarCedula([FromBody] EnviarCedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(cedula),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PutAsync($"{_apiUrls.TransporteUrl}api/transporte/cedulaEvaluacion/enviarCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaTransporteDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaTransporteDto> DBloquearCedula([FromBody] DBloquearCedulaUpdateCommand cedula)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(cedula),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PutAsync($"{_apiUrls.TransporteUrl}api/transporte/cedulaEvaluacion/dbloquearCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaTransporteDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaTransporteDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(cedula),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PutAsync($"{_apiUrls.TransporteUrl}api/transporte/cedulaEvaluacion/updateCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaTransporteDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

    }
}
