using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Microbiologicos.Contratos.Queries;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Microbiologicos.Contratos.Queries
{
    public interface IQContratoMicrobiologicosProxy
    {
        Task<List<ContratoDto>> GetAllAsync();
        Task<ContratoDto> GetContratoByIdAsync(int contrato);
    }

    public class QContratoMicrobiologicosProxy : IQContratoMicrobiologicosProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QContratoMicrobiologicosProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<ContratoDto>> GetAllAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}microbiologicos/contratos/getContratos");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<ContratoDto> GetContratoByIdAsync(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}microbiologicos/contratos/getContratoById/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
