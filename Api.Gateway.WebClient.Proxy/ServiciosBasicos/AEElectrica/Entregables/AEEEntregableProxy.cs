using Api.Gateway.Models.Entregables.ServiciosBasicos.Commands;
using Api.Gateway.Models.Entregables.ServiciosBasicos.DTOs;
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

namespace Api.Gateway.WebClient.Proxy.ServiciosBasicos.AEElectrica.Entregables
{
    public interface IAEEEntregableProxy
    {
        Task<List<EntregableSBDto>> GetAllEntregables();
        Task<List<EntregableSBDto>> GetEntregablesBySolicitud(int solicitud);
        Task<EntregableSBDto> GetEntregableById(int entregable);
        Task<int> UpdateEntregable([FromForm] EntregableSBUpdateCommand entregable);
        Task<int> SeguimientoEntregable([FromForm] AEntregableSBUpdateCommand entregable);
        Task<string> VisualizarEntregables(int solicitud, string tipo, string archivo);
    }

    public class AEEEntregableProxy : IAEEEntregableProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public AEEEntregableProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<EntregableSBDto>> GetAllEntregables()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/entregables");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EntregableSBDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<EntregableSBDto>> GetEntregablesBySolicitud(int solicitud)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/entregables/getEntregablesBySolicitud/{solicitud}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EntregableSBDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<EntregableSBDto> GetEntregableById(int entregable)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/entregables/getEntregableById/{entregable}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EntregableSBDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> UpdateEntregable([FromForm] EntregableSBUpdateCommand entregable)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(entregable.Id.ToString()), "Id");
            formContent.Add(new StringContent(entregable.SolicitudId.ToString()), "SolicitudId");
            formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");

            if (entregable.Archivo != null)
            {
                formContent.Add(new StringContent(entregable.Anio.ToString()), "Anio");
                formContent.Add(new StringContent(entregable.Mes.ToString()), "Mes");
                formContent.Add(new StringContent(entregable.TipoEntregable.ToString()), "TipoEntregable");
                var fileStreamContentPDF = new StreamContent(entregable.Archivo.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(entregable.Archivo.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Archivo", entregable.Archivo.FileName);
                formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");
            }

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}aeelectrica/entregables/updateEntregable", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> SeguimientoEntregable([FromBody] AEntregableSBUpdateCommand entregable)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(entregable),
                   Encoding.UTF8,
                   "application/json"
                   );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}aeelectrica/entregables/seguimientoEntregable", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<string> VisualizarEntregables(int solicitud, string tipo, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/entregables/visualizarEntregable/{solicitud}/{tipo}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;
        }
    }
}
