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

namespace Api.Gateway.Proxies.Convencional.ServiciosContrato.Queries
{
    public interface IQSContratoConvencionalProxy
    {
        Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato);
    }

    public class QSContratoConvencionalProxy : IQSContratoConvencionalProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QSContratoConvencionalProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ConvencionalUrl}api/convencional/servicioContrato/getServiciosContrato/{contrato}");
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
