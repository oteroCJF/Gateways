using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Comedor.Commands;
using Api.Gateway.Models.Incidencias.Comedor.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Comedor.Incidencias.Commands
{
    public interface ICIncidenciaComedorProxy
    {
        Task CreateIncidencia([FromBody] CIncidenciaCreateCommand command);
        Task UpdateIncidencia([FromBody] CIncidenciaUpdateCommand command);
        Task<int> DeleteIncidencias([FromBody] CIncidenciaDeleteCommand command);
        Task<int> DeleteIncidencia([FromBody] CIncidenciaDeleteCommand command);
    }

    public class CIncidenciaComedorProxy : ICIncidenciaComedorProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CIncidenciaComedorProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task CreateIncidencia([FromBody] CIncidenciaCreateCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/insertaIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task UpdateIncidencia([FromBody] CIncidenciaUpdateCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PutAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/actualizarIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task<int> DeleteIncidencias([FromBody] CIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/eliminarIncidencias", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/eliminarIncidencia", content);
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
