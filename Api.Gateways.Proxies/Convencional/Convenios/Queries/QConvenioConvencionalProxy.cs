using Api.Gateway.Models.Convenios.Commands;
using Api.Gateway.Models.Convenios.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Proxies.Convencional.Convenios.Commands;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Convencional.Convenios.Queries
{
    public interface IQConvenioConvencionalProxy
    {
        Task<List<ConvenioDto>> GetConveniosByContrato(int contrato);
        Task<ConvenioDto> GetConvenioByIdAsync(int convenio);
        Task<List<RubroConvenioDto>> GetRubrosByConvenio(int convenio);
    }

    public class QConvenioConvencionalProxy : IQConvenioConvencionalProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QConvenioConvencionalProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<ConvenioDto>> GetConveniosByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ConvencionalUrl}api/convencional/convenios/getConveniosByContrato/{contrato}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ConvencionalUrl}api/convencional/convenios/getConvenioById/{convenio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ConvenioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<RubroConvenioDto>> GetRubrosByConvenio(int convenio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ConvencionalUrl}api/convencional/convenios/getRubrosByConvenio/{convenio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<RubroConvenioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
