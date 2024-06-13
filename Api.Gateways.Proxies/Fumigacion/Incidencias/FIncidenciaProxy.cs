using Api.Gateway.Models.Incidencias.Fumigacion.DTOs;
using Api.Gateway.Models.Incidencias.Fumigacion.Commands;
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
using System.Diagnostics.Contracts;

namespace Api.Gateway.Proxies.Fumigacion.Incidencias
{
    public interface IFIncidenciaProxy
    {
        Task<List<FIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta);
        Task<FIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<FConfiguracionIncidenciaDto>> GetConfiguracionIncidencias();
        Task<FConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta);
        Task CreateIncidencia([FromForm] FIncidenciaCreateCommand command);
        Task UpdateIncidencia([FromForm] FIncidenciaUpdateCommand command);
        Task<int> DeleteIncidencias([FromBody] FIncidenciaDeleteCommand command);
        Task<int> DeleteIncidencia([FromBody] FIncidenciaDeleteCommand command);
        Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo);
    }

    public class FIncidenciaProxy : IFIncidenciaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public FIncidenciaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<FIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/incidenciasCedula/getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<FIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/incidenciasCedula/getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<FIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<FConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/incidenciasCedula/getConfiguracionIncidencias");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FConfiguracionIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<FConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/incidenciasCedula/getConfiguracionIncidenciasByPregunta/{pregunta}/{respuesta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<FConfiguracionIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task CreateIncidencia([FromBody] FIncidenciaCreateCommand incidencia)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(incidencia.UsuarioId != null ? incidencia.UsuarioId.ToString() : ""), "UsuarioId");
            formContent.Add(new StringContent(incidencia.CedulaEvaluacionId.ToString()), "CedulaEvaluacionId");
            formContent.Add(new StringContent(incidencia.IncidenciaId.ToString()), "IncidenciaId");
            formContent.Add(new StringContent(incidencia.MesId.ToString()), "MesId");
            formContent.Add(new StringContent(incidencia.Pregunta.ToString()), "Pregunta");
            formContent.Add(new StringContent(incidencia.DIncidenciaId.ToString()), "DIncidenciaId");
            formContent.Add(new StringContent(incidencia.FechaProgramada != null ? incidencia.FechaProgramada.ToString() : "1990-01-01"), "FechaProgramada");
            formContent.Add(new StringContent(incidencia.FechaRealizada != null ? incidencia.FechaRealizada.ToString() : "1990-01-01"), "FechaRealizada");
            formContent.Add(new StringContent(incidencia.FechaReaparicion != null ? incidencia.FechaReaparicion.ToString() : "1990-01-01"), "FechaReaparicion");
            formContent.Add(new StringContent(incidencia.HoraProgramada != null ? incidencia.HoraProgramada.ToString() : "00:00:00"), "HoraProgramada");
            formContent.Add(new StringContent(incidencia.HoraRealizada != null ? incidencia.HoraRealizada.ToString() : "00:00:00"), "HoraRealizada");
            formContent.Add(new StringContent(incidencia.Observaciones != null ? incidencia.Observaciones.ToString() : ""), "Observaciones");

            var request = await _httpClient.PostAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/incidenciasCedula/insertaIncidencia", formContent);
            request.EnsureSuccessStatusCode();
        }
        
        public async Task UpdateIncidencia([FromBody] FIncidenciaUpdateCommand incidencia)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(incidencia.Id.ToString()), "Id");
            formContent.Add(new StringContent(incidencia.UsuarioId != null ? incidencia.UsuarioId.ToString() : ""), "UsuarioId");
            formContent.Add(new StringContent(incidencia.CedulaEvaluacionId.ToString()), "CedulaEvaluacionId");
            formContent.Add(new StringContent(incidencia.IncidenciaId.ToString()), "IncidenciaId");
            formContent.Add(new StringContent(incidencia.MesId.ToString()), "MesId");
            formContent.Add(new StringContent(incidencia.Pregunta.ToString()), "Pregunta");
            formContent.Add(new StringContent(incidencia.DIncidenciaId.ToString()), "DIncidenciaId");
            formContent.Add(new StringContent(incidencia.FechaProgramada != null ? incidencia.FechaProgramada.ToString() : "1990-01-01"), "FechaProgramada");
            formContent.Add(new StringContent(incidencia.FechaRealizada != null ? incidencia.FechaRealizada.ToString() : "1990-01-01"), "FechaRealizada");
            formContent.Add(new StringContent(incidencia.FechaReaparicion != null ? incidencia.FechaReaparicion.ToString() : "1990-01-01"), "FechaReaparicion");
            formContent.Add(new StringContent(incidencia.HoraProgramada != null ? incidencia.HoraProgramada.ToString() : "00:00:00"), "HoraProgramada");
            formContent.Add(new StringContent(incidencia.HoraRealizada != null ? incidencia.HoraRealizada.ToString() : "00:00:00"), "HoraRealizada");
            formContent.Add(new StringContent(incidencia.Observaciones != null ? incidencia.Observaciones.ToString() : ""), "Observaciones");

            var request = await _httpClient.PutAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/incidenciasCedula/actualizarIncidencia", formContent);
            request.EnsureSuccessStatusCode();
        }

        public async Task<int> DeleteIncidencias([FromBody] FIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/incidenciasCedula/eliminarIncidencias", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<int> DeleteIncidencia([FromBody] FIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(incidencia),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/incidenciasCedula/eliminarIncidencia", content);
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
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/incidenciasCedula/visualizarActas/{anio}/{mes}/{folio}/{tipo}/{tipoArchivo}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
