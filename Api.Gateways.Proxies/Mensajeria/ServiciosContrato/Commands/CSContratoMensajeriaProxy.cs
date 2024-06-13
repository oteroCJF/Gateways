using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Models.Contratos.Commands.ServicioContrato;

namespace Api.Gateway.Proxies.Mensajeria.ServiciosContrato.Commands
{
    public interface ICSContratoMensajeriaProxy
    {
        Task<ServicioContratoDto> CreateServicioContrato([FromBody] ServicioContratoCreateCommand command);
        Task<ServicioContratoDto> UpdateServicioContrato([FromBody] ServicioContratoUpdateCommand command);
        Task<int> DeleteServicioContrato([FromBody] ServicioContratoDeleteCommand command);
    }

    public class CSContratoMensajeriaProxy : ICSContratoMensajeriaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CSContratoMensajeriaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<ServicioContratoDto> CreateServicioContrato([FromBody] ServicioContratoCreateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/servicioContrato/createSContrato", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ServicioContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<ServicioContratoDto> UpdateServicioContrato([FromBody] ServicioContratoUpdateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/servicioContrato/updateSContrato", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ServicioContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> DeleteServicioContrato([FromBody] ServicioContratoDeleteCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
            "application/json"
            );
            var request = await _httpClient.PutAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/servicioContrato/deleteSContrato", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
