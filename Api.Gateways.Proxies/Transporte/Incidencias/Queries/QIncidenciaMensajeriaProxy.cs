using Api.Gateway.Models.Incidencias.Transporte.Commands;
using Api.Gateway.Models.Incidencias.Transporte.DTOs;
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

namespace Api.Gateway.Proxies.Transporte.Incidencias.Queries
{
    public interface IQIncidenciaTransporteProxy
    {
        Task<List<TIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<TIncidenciaDto>> GetIncidenciasByCedula(int cedula);
        Task<TIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<TConfiguracionIncidenciaDto>> GetConfiguracionIncidencias();
        Task<TConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta);
        Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo);
    }

    public class QIncidenciaTransporteProxy : IQIncidenciaTransporteProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QIncidenciaTransporteProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<TIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<TIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<TIncidenciaDto>> GetIncidenciasByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/getIncidenciasByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<TIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<TIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<TConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/getConfiguracionIncidencias");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<TConfiguracionIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<TConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/getConfiguracionIncidenciasByPregunta/{pregunta}/{respuesta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TConfiguracionIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/incidenciasCedula/visualizarActas/{anio}/{mes}/{folio}/{tipo}/{tipoArchivo}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
