using Api.Gateway.Models.Contratos.Commands.ServicioContrato;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Microbiologicos.ServiciosContrato.Queries;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Microbiologicos.ServiciosContrato.Queries
{
    public interface IQSContratoMicrobiologicosProxy
    {
        Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato);
    }

    public class QSContratoMicrobiologicosProxy : IQSContratoMicrobiologicosProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QSContratoMicrobiologicosProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}microbiologicos/servicioContrato/getServiciosContrato/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ServicioContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
