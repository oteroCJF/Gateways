using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.Models.Contratos.DTOs;
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

namespace Api.Gateway.WebClient.Proxy.BMuebles.Contratos.Queries
{
    public interface IQBMContratoProxy
    {

        Task<List<ContratoDto>> GetAllAsync();
        Task<ContratoDto> GetContratoActivo();
        Task<ContratoDto> GetContratoByIdAsync(int contrato);
    }

    public class QBMContratoProxy : IQBMContratoProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QBMContratoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<ContratoDto>> GetAllAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}bmuebles/contratos/getContratos");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<ContratoDto> GetContratoActivo()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}bmuebles/contratos/getContratoActivo");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<ContratoDto> GetContratoByIdAsync(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}bmuebles/contratos/getContratoById/{contrato}");
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
