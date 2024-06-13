using Api.Gateway.Models.Entregables.ServiciosBasicos.Commands;
using Api.Gateway.Models.Entregables.ServiciosBasicos.DTOs;
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

namespace Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.Entregables
{
    public interface IAEEEntregableProxy
    {
        Task<List<EntregableSBDto>> GetAllEntregables();
        Task<List<EntregableSBDto>> GetEntregablesBySolicitud(int solicitud);
        Task<EntregableSBDto> GetEntregableById(int entregable);
        Task<int> UpdateEntregable([FromForm] EntregableSBUpdateCommand entregable);
        Task<int> SeguimientoEntregable([FromBody] AEntregableSBUpdateCommand entregable);
        Task<string> VisualizarEntregable(string ruta, string archivo);
    }

    public class AEEEntregableProxy : IAEEEntregableProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public AEEEntregableProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<EntregableSBDto>> GetAllEntregables()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/entregables");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/entregables/getEntregablesBySolicitud/{solicitud}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/entregables/getEntregableById/{entregable}");
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
            formContent.Add(new StringContent(entregable.EstatusId.ToString()), "EstatusId");
            formContent.Add(new StringContent(entregable.SolicitudId.ToString()), "SolicitudId");
            formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(entregable.EntregableId.ToString()), "EntregableId");

            if (entregable.Archivo != null)
            {
                formContent.Add(new StringContent(entregable.TipoEntregable.ToString()), "TipoEntregable");
                formContent.Add(new StringContent(entregable.Anio.ToString()), "Anio");
                formContent.Add(new StringContent(entregable.Mes.ToString()), "Mes");
                var fileStreamContentPDF = new StreamContent(entregable.Archivo.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(entregable.Archivo.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Archivo", entregable.Archivo.FileName);
                formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");
            }

            var request = await _httpClient.PutAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/entregables/updateEntregable", formContent);
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

            var request = await _httpClient.PutAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/entregables/seguimientoEntregable", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<string> VisualizarEntregable(string ruta, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/entregables/visualizarEntregable/{ruta}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
