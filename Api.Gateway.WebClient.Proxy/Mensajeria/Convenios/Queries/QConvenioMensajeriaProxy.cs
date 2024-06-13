using Api.Gateway.Models;
using Api.Gateway.Models.Convenios.Commands;
using Api.Gateway.Models.Convenios.DTOs;
using Api.Gateway.Proxies.Mensajeria.Convenios.Queries;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Mensajeria.Convenios.Queries
{
    public interface IQConvenioMensajeriaProxy
    {
        Task<List<ConvenioDto>> GetConveniosByContrato(int contrato);
        Task<ConvenioDto> GetConvenioByIdAsync(int convenio);
    }

    public class QConvenioMensajeriaProxy : IQConvenioMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QConvenioMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<ConvenioDto>> GetConveniosByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/convenios/getConveniosByContrato/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ConvenioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<ConvenioDto> GetConvenioByIdAsync(int convenio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/convenios/getConvenioById/{convenio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ConvenioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
