using System.Net.Http;
using System.Net.Http.Headers;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using System.Text;
using Api.Gateway.Models;
using Api.Gateway.Proxies.Agua.Entregables.Commands;

namespace Api.Gateway.Proxies.Agua.Entregables
{
    public interface IQEntregableAguaProxy
    {
        Task<List<EntregableDto>> GetAllEntregablesAsync();
        Task<List<EntregableDto>> GetEntregablesByCedula(int cedula);
        Task<EntregableDto> GetEntregableById(int entregable);
        Task<string> VisualizarEntregable(int anio, string mes, string folio, string archivo, string tipo);
        Task<string> GetPathEntregables();
        Task<DataCollection<EntregableDto>> GetEntregablesValidados();
    }
    public class QEntregableAguaProxy : IQEntregableAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QEntregableAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<EntregableDto>> GetAllEntregablesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/entregables");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<EntregableDto>> GetEntregablesByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/entregables/getEntregablesByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<EntregableDto> GetEntregableById(int entregable)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/entregables/getEntregableById/{entregable}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EntregableDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<DataCollection<EntregableDto>> GetEntregablesValidados()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/entregables/getEntregablesValidados");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DataCollection<EntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<string> VisualizarEntregable(int anio, string mes, string folio, string archivo, string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/entregables/visualizarEntregable/{anio}/{mes}/{folio}/{archivo}/{tipo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }

        public async Task<string> GetPathEntregables()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/entregables/getPathEntregables");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
