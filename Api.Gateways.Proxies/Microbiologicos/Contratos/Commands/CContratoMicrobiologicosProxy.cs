using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Microbiologicos.Contratos.Commands
{
    public interface ICContratoMicrobiologicosProxy
    {
        Task<int> CreateContrato([FromBody] ContratoCreateCommand command);
        Task<int> UpdateContrato([FromBody] ContratoUpdateCommand command);
        Task<int> DeleteContrato([FromBody] ContratoDeleteCommand command);
    }

    public class CContratoMicrobiologicosProxy : ICContratoMicrobiologicosProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CContratoMicrobiologicosProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<int> CreateContrato([FromForm] ContratoCreateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.MicrobiologicosUrl}api/microbiologicos/contratos/createContrato", content);
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

            var request = await _httpClient.PutAsync($"{_apiUrls.MicrobiologicosUrl}api/microbiologicos/contratos/updateContrato", content);
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

            var request = await _httpClient.PutAsync($"{_apiUrls.MicrobiologicosUrl}api/microbiologicos/contratos/deleteContrato", content);
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

