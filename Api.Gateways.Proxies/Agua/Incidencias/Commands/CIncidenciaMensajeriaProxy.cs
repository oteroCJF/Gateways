using Api.Gateway.Models.Incidencias.Agua.Commands;
using Api.Gateway.Models.Incidencias.Agua.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Agua.Incidencias.Commands
{
    public interface ICIncidenciaAguaProxy
    {
        Task CreateIncidencia([FromBody] AIncidenciaCreateCommand command);
        Task UpdateIncidencia([FromBody] AIncidenciaUpdateCommand command);
        Task<int> DeleteIncidencias([FromBody] AIncidenciaDeleteCommand command);
        Task<int> DeleteIncidencia([FromBody] AIncidenciaDeleteCommand command);
    }

    public class CIncidenciaAguaProxy : ICIncidenciaAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CIncidenciaAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task CreateIncidencia([FromBody] AIncidenciaCreateCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/incidenciasCedula/insertaIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task UpdateIncidencia([FromBody] AIncidenciaUpdateCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PutAsync($"{_apiUrls.AguaUrl}api/agua/incidenciasCedula/actualizarIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task<int> DeleteIncidencias([FromBody] AIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/incidenciasCedula/eliminarIncidencias", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/incidenciasCedula/eliminarIncidencia", content);
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
