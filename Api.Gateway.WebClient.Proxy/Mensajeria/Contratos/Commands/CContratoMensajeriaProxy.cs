using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Mensajeria.Contratos.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Mensajeria.Contratos.Commands
{
    public interface ICContratoMensajeriaProxy
    {
        Task<int> CreateContrato([FromBody] ContratoCreateCommand command);
        Task<int> UpdateContrato([FromBody] ContratoUpdateCommand command);
        Task<int> DeleteContrato([FromBody] ContratoDeleteCommand command);
    }

    public class CContratoMensajeriaProxy : ICContratoMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CContratoMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<int> CreateContrato([FromForm] ContratoCreateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
                   "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/contratos/createContrato", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> UpdateContrato([FromForm] ContratoUpdateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
                   "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}mensajeria/contratos/updateContrato", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> DeleteContrato([FromForm] ContratoDeleteCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
                   "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}mensajeria/contratos/deleteContrato", content);
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
