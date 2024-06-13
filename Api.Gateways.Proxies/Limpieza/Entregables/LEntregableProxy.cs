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
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;

namespace Api.Gateway.Proxies.Limpieza.Entregables
{
    public interface ILEntregableProxy
    {
        Task<List<EntregableDto>> GetAllEntregablesAsync();
        Task<List<EntregableDto>> GetEntregablesByCedula(int cedula);
        Task<EntregableDto> GetEntregableById(int entregable);
        Task UpdateEntregable([FromForm] EntregableCommandUpdate entregable);
        Task AUpdateEntregable([FromForm] EEntregableUpdateCommand entregable);
        Task UpdateEntregableSR([FromForm] ESREntregableUpdateCommand entregable);
        Task<string> VisualizarEntregable(int anio, string mes, string folio, string archivo, string tipo);

        Task<string> GetPathEntregables();
    }
    public class LEntregableProxy : ILEntregableProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public LEntregableProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<EntregableDto>> GetAllEntregablesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregables");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregables/getEntregablesByCedula/{cedula}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregables/getEntregableById/{entregable}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EntregableDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task UpdateEntregable([FromForm] EntregableCommandUpdate entregable)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(entregable.Id.ToString()), "Id");
            formContent.Add(new StringContent(entregable.EstatusId.ToString()), "EstatusId");

            if (entregable.Archivo != null)
            {
                formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");
                formContent.Add(new StringContent(entregable.TipoEntregable.ToString()), "TipoEntregable");
                formContent.Add(new StringContent(entregable.Anio.ToString()), "Anio");
                formContent.Add(new StringContent(entregable.Mes.ToString()), "Mes");
                formContent.Add(new StringContent(entregable.Folio.ToString()), "Folio");
                var fileStreamContentPDF = new StreamContent(entregable.Archivo.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(entregable.Archivo.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Archivo", entregable.Archivo.FileName);
                formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");
            }

            if (entregable.Validar)
            {
                formContent.Add(new StringContent(entregable.Validado.ToString()), "Validado");
                formContent.Add(new StringContent(entregable.Validar.ToString()), "Validar");
            }

            var request = await _httpClient.PutAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregables/actualizarEntregable", formContent);
            request.EnsureSuccessStatusCode();
        }

        public async Task AUpdateEntregable([FromForm] EEntregableUpdateCommand entregable)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(entregable.Id.ToString()), "Id");
            formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(entregable.CedulaEvaluacionId.ToString()), "CedulaEvaluacionId");
            formContent.Add(new StringContent(entregable.EntregableId.ToString()), "EntregableId");
            if (entregable.Estatus != null)
                formContent.Add(new StringContent(entregable.Estatus.ToString()), "Estatus");
            formContent.Add(new StringContent(entregable.EstatusId.ToString()), "EstatusId");
            formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");

            var request = await _httpClient.PutAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregables/autorizarEntregable", formContent);
            request.EnsureSuccessStatusCode();
        }

        public async Task UpdateEntregableSR([FromBody] ESREntregableUpdateCommand entregable)
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(entregable),
                 Encoding.UTF8,
                 "application/json"
             );

            var request = await _httpClient.PutAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregables/updateEntregableSR", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task<string> VisualizarEntregable(int anio, string mes, string folio, string archivo, string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregables/visualizarEntregable/{anio}/{mes}/{folio}/{archivo}/{tipo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }

        public async Task<string> GetPathEntregables()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregables/getPathEntregables");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;
        }

    }
}
