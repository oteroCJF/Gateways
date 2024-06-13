using Api.Gateway.Models.Incidencias.Transporte.Commands;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Transporte.Incidencias.Commands
{
    public interface ICIncidenciaTransporteProxy
    {
        Task CreateIncidencia([FromForm] TIncidenciaCreateCommand command);
        Task UpdateIncidencia([FromForm] TIncidenciaUpdateCommand command);
        Task<int> DeleteIncidencias([FromBody] TIncidenciaDeleteCommand command);
        Task<int> DeleteIncidencia([FromBody] TIncidenciaDeleteCommand command);
    }

    public class CIncidenciaTransporteProxy : ICIncidenciaTransporteProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CIncidenciaTransporteProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task CreateIncidencia([FromBody] TIncidenciaCreateCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/insertaIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task UpdateIncidencia([FromForm] TIncidenciaUpdateCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PutAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/actualizarIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task<int> DeleteIncidencias([FromBody] TIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/eliminarIncidencias", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> DeleteIncidencia([FromBody] TIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/eliminarIncidencia", content);
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
