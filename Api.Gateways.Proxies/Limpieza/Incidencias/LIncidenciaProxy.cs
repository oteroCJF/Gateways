using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Mensajeria.Commands;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Limpieza.Incidencias
{
    public interface ILIncidenciaProxy
    {
        Task<List<LIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta);
        Task<LIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<LConfiguracionIncidenciaDto>> GetConfiguracionIncidencias();
        Task<LConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta);
        Task CreateIncidencia(LIncidenciaCreateCommand command);
        Task UpdateIncidencia(LIncidenciaUpdateCommand command);
        Task<int> DeleteIncidencias([FromBody] LIncidenciaDeleteCommand command);
        Task<int> DeleteIncidencia([FromBody] LIncidenciaDeleteCommand command);
        Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo);
    }

    public class LIncidenciaProxy : ILIncidenciaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public LIncidenciaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<LIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/incidenciasCedula/getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/incidenciasCedula/getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/incidenciasCedula/getConfiguracionIncidencias");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/incidenciasCedula/getConfiguracionIncidenciasByPregunta/{pregunta}/{respuesta}");
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/incidenciasCedula/insertaIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task UpdateIncidencia(LIncidenciaUpdateCommand incidencia)
        {
            var content = new StringContent(
                     JsonSerializer.Serialize(incidencia),
                     Encoding.UTF8,
                     "application/json"
                 );

            var request = await _httpClient.PutAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/incidenciasCedula/actualizarIncidencia", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task<int> DeleteIncidencias([FromBody] LIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/incidenciasCedula/eliminarIncidencias", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/incidenciasCedula/eliminarIncidencia", content);
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
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/incidenciasCedula/visualizarActas/{anio}/{mes}/{folio}/{tipo}/{tipoArchivo}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
