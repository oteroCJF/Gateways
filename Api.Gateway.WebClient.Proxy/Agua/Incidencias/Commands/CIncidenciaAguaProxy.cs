using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Agua.Commands;
using Api.Gateway.Models.Incidencias.Agua.DTOs;
using Api.Gateway.Proxies.Agua.Incidencias.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Agua.Incidencias.Commands
{
    public interface ICIncidenciaAguaProxy
    {
        Task CreateIncidencia([FromBody] AIncidenciaCreateCommand command);
        Task UpdateIncidencia([FromBody] AIncidenciaUpdateCommand command);
        Task<int> DeleteIncidencias([FromBody] AIncidenciaDeleteCommand incidencia);
        Task<int> DeleteIncidencia([FromBody] AIncidenciaDeleteCommand incidencia);
    }

    public class CIncidenciaAguaProxy : ICIncidenciaAguaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CIncidenciaAguaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }


        public async Task CreateIncidencia([FromBody] AIncidenciaCreateCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}agua/incidenciasCedula/insertaIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task UpdateIncidencia([FromBody] AIncidenciaUpdateCommand incidencia)
        {

            var content = new StringContent(
                 JsonSerializer.Serialize(incidencia),
                 Encoding.UTF8,
                 "application/json"
             );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}agua/incidenciasCedula/actualizarIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task<int> DeleteIncidencias([FromBody] AIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}agua/incidenciasCedula/eliminarIncidencias", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> DeleteIncidencia([FromBody] AIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}agua/incidenciasCedula/eliminarIncidencia", content);
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
