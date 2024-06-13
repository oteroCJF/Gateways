using Api.Gateway.Models.Contratos.Commands.ServicioContrato;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Fumigacion.ServiciosContrato
{
    public interface IFServicioContratoProxy 
    {
        Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato);
        Task<ServicioContratoDto> CreateServicioContrato([FromBody] ServicioContratoCreateCommand command);
        Task<ServicioContratoDto> UpdateServicioContrato([FromBody] ServicioContratoUpdateCommand command);
        Task<int> DeleteServicioContrato([FromBody] ServicioContratoDeleteCommand command);
    }

    public class FServicioContratoProxy : IFServicioContratoProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public FServicioContratoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/servicioContrato/getServiciosContrato/{contrato}");
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

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}fumigacion/servicioContrato/createSContrato", content);
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

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}fumigacion/servicioContrato/updateSContrato", content);
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
            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}fumigacion/servicioContrato/deleteSContrato", content);
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
