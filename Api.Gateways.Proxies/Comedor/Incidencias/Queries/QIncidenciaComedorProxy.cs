using Api.Gateway.Models.Incidencias.Comedor.Commands;
using Api.Gateway.Models.Incidencias.Comedor.DTOs;
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

namespace Api.Gateway.Proxies.Comedor.Incidencias.Queries
{
    public interface IQIncidenciaComedorProxy
    {
        Task<List<CIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<CIncidenciaDto>> GetIncidenciasByCedula(int cedula);
        Task<List<DetalleIncidenciaDto>> GetDetalleIncidenciasById(int cedula);
        Task<CIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<CConfiguracionIncidenciaDto>> GetConfiguracionIncidencias();
        Task<CConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta);
        Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo);
    }

    public class QIncidenciaComedorProxy : IQIncidenciaComedorProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QIncidenciaComedorProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CIncidenciaDto>> GetIncidenciasByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/getIncidenciasByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<DetalleIncidenciaDto>> GetDetalleIncidenciasById(int incidencias)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/getDetalleIncidenciasById/{incidencias}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<DetalleIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<CIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/getConfiguracionIncidencias");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CConfiguracionIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/getConfiguracionIncidenciasByPregunta/{pregunta}/{respuesta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CConfiguracionIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/incidenciasCedula/visualizarActas/{anio}/{mes}/{folio}/{tipo}/{tipoArchivo}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
