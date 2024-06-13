using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Microbiologicos.Contratos.Commands
{
    public interface ICContratoMicrobiologicosProxy
    {
        Task<int> CreateContrato([FromBody] ContratoCreateCommand command);
        Task<int> UpdateContrato([FromBody] ContratoUpdateCommand command);
        Task<int> DeleteContrato([FromBody] ContratoDeleteCommand command);
    }

    public class CContratoMicrobiologicosProxy : ICContratoMicrobiologicosProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CContratoMicrobiologicosProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
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

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}microbiologicos/contratos/createContrato", content);
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

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}microbiologicos/contratos/updateContrato", content);
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

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}microbiologicos/contratos/deleteContrato", content);
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
