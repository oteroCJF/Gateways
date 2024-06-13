using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Comedor.Commands;
using Api.Gateway.Models.Incidencias.Comedor.DTOs;
using Api.Gateway.Proxies.Comedor.Incidencias.Commands;
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

namespace Api.Gateway.WebClient.Proxy.Comedor.Incidencias.Commands
{
    public interface ICIncidenciaComedorProxy
    {
        Task CreateIncidencia([FromBody] CIncidenciaCreateCommand command);
        Task UpdateIncidencia([FromBody] CIncidenciaUpdateCommand command);
        Task<int> DeleteIncidencias([FromBody] CIncidenciaDeleteCommand incidencia);
        Task<int> DeleteIncidencia([FromBody] CIncidenciaDeleteCommand incidencia);
    }

    public class CIncidenciaComedorProxy : ICIncidenciaComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CIncidenciaComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }


        public async Task CreateIncidencia([FromBody] CIncidenciaCreateCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}comedor/incidenciasCedula/insertaIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task UpdateIncidencia([FromBody] CIncidenciaUpdateCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}comedor/incidenciasCedula/actualizarIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task<int> DeleteIncidencias([FromBody] CIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}comedor/incidenciasCedula/eliminarIncidencias", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> DeleteIncidencia([FromBody] CIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}comedor/incidenciasCedula/eliminarIncidencia", content);
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
