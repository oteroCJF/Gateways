using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Transporte.Contratos.Queries
{
    public interface IQContratoTransporteProxy
    {
        Task<List<ContratoDto>> GetAllContratosAsync();
        Task<ContratoDto> GetContratoByIdAsync(int contrato);
    }

    public class QContratoTransporteProxy : IQContratoTransporteProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QContratoTransporteProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<ContratoDto>> GetAllContratosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/contratos/getContratos");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/contratos/getContratoById/{contrato}");
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

