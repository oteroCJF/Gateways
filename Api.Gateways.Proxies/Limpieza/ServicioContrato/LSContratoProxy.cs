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

namespace Api.Gateway.Proxies.Limpieza.ServicioContrato
{
    public interface ILServicioContratoProxy
    {
        Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato);
        Task<ServicioContratoDto> CreateServicioContrato([FromBody] ServicioContratoCreateCommand command);
        Task<ServicioContratoDto> UpdateServicioContrato([FromBody] ServicioContratoUpdateCommand command);
        Task<int> DeleteServicioContrato([FromBody] ServicioContratoDeleteCommand command);
    }

    public class LSContratoProxy : ILServicioContratoProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public LSContratoProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/servicioContrato/getServiciosContrato/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ServicioContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<ServicioContratoDto> CreateServicioContrato([FromBody] ServicioContratoCreateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/servicioContrato/createSContrato", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ServicioContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<ServicioContratoDto> UpdateServicioContrato([FromBody] ServicioContratoUpdateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/servicioContrato/updateSContrato", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ServicioContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> DeleteServicioContrato([FromBody] ServicioContratoDeleteCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
            "application/json"
            );
            var request = await _httpClient.PutAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/servicioContrato/deleteSContrato", content);
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
