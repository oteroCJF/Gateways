using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Limpieza.Incidencias
{
    public interface ILIncidenciaProxy
    {
        Task<List<LIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta);
        Task<LIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<LConfiguracionIncidenciaDto>> GetConfiguracionIncidencias();
        Task<LConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta);
        Task CreateIncidencia(LIncidenciaCreateCommand incidencia);
        Task UpdateIncidencia(LIncidenciaUpdateCommand incidencia);
        Task<int> DeleteIncidencias([FromBody] LIncidenciaDeleteCommand incidencia);
        Task<int> DeleteIncidencia([FromBody] LIncidenciaDeleteCommand incidencia);
        Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo);
    }

    public class LIncidenciaProxy : ILIncidenciaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public LIncidenciaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<LIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/incidenciasCedula/getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<LIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/incidenciasCedula/getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<LIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<LConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/incidenciasCedula/getConfiguracionIncidencias");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LConfiguracionIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<LConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/incidenciasCedula/getConfiguracionIncidenciasByPregunta/{pregunta}/{respuesta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<LConfiguracionIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task CreateIncidencia(LIncidenciaCreateCommand incidencia)
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(incidencia),
                 Encoding.UTF8,
                 "application/json"
             );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/incidenciasCedula/insertaIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task UpdateIncidencia(LIncidenciaUpdateCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}limpieza/incidenciasCedula/actualizarIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task<int> DeleteIncidencias([FromBody] LIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/incidenciasCedula/eliminarIncidencias", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> DeleteIncidencia([FromBody] LIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/incidenciasCedula/eliminarIncidencia", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/incidenciasCedula/visualizarActas/{anio}/{mes}/{folio}/{tipo}/{tipoArchivo}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
