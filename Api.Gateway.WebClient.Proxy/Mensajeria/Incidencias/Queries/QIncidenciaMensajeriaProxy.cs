using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Mensajeria.Commands;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using Api.Gateway.Proxies.Mensajeria.Incidencias.Queries;
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

namespace Api.Gateway.WebClient.Proxy.Mensajeria.Incidencias.Queries
{
    public interface IQIncidenciaMensajeriaProxy
    {
        Task<List<MIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<MIncidenciaDto>> GetIncidenciasByCedula(int cedula);
        Task<MIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<MConfiguracionIncidenciaDto>> GetConfiguracionIncidencias();
        Task<MConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta);
        Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo);
    }

    public class QIncidenciaMensajeriaProxy : IQIncidenciaMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QIncidenciaMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<MIncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<MIncidenciaDto>> GetIncidenciasByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/getIncidenciasByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<MIncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<MConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/getConfiguracionIncidencias");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MConfiguracionIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<MConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/getConfiguracionIncidenciasByPregunta/{pregunta}/{respuesta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MConfiguracionIncidenciaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/visualizarActas/{anio}/{mes}/{folio}/{tipo}/{tipoArchivo}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
