using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Models.Estatus.DTOs.EntregablesEstatus;
using Api.Gateway.Proxies.Comedor.Entregables.Queries;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.Entregables.Queries
{
    public interface IQEntregableComedorProxy
    {
        Task<List<EntregableDto>> GetEntregablesByCedula(int cedula);
        Task<List<EntregableEstatusDto>> GetEntregablesByEstatus(int estatus);
        Task<string> VisualizarEntregables(int anio, string mes, string folio, string archivo, string tipo);
    }

    public class QEntregableComedorProxy : IQEntregableComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QEntregableComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<EntregableEstatusDto>> GetEntregablesByEstatus(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/entregablesCedula/getEntregablesByEstatus/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EntregableEstatusDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<EntregableDto>> GetEntregablesByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/entregablesCedula/getEntregablesByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<string> VisualizarEntregables(int anio, string mes, string folio, string archivo, string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/entregablesCedula/visualizarEntregable/{anio}/{mes}/{folio}/{archivo}/{tipo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;
        }
    }
}
