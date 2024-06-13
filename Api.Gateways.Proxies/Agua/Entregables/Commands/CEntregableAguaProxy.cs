using System.Net.Http;
using System.Net.Http.Headers;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using System.Text;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Create;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Delete;

namespace Api.Gateway.Proxies.Agua.Entregables.Commands
{
    public interface ICEntregableAguaProxy
    {
        Task<EntregableDto> CreateEntregable([FromBody] EntregableCreateCommand entregable);
        Task UpdateEntregable([FromForm] EntregableCommandUpdate entregable);
        Task<EntregableDto> DeleteEntregable([FromBody] EntregableDeleteCommand entregable);
        Task AUpdateEntregable(EEntregableUpdateCommand entregable);
        Task UpdateEntregableSR([FromForm] ESREntregableUpdateCommand entregable);
    }
    public class CEntregableAguaProxy : ICEntregableAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CEntregableAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<EntregableDto> CreateEntregable([FromBody] EntregableCreateCommand entregable)
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(entregable),
                 Encoding.UTF8,
                 "application/json"
             );

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/entregables/createEntregable", content);
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
            formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");
            formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");

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
            }

            if (entregable.Validar)
            {
                formContent.Add(new StringContent(entregable.Validado.ToString()), "Validado");
                formContent.Add(new StringContent(entregable.Validar.ToString()), "Validar");
            }

            var request = await _httpClient.PutAsync($"{_apiUrls.AguaUrl}api/agua/entregables/actualizarEntregable", formContent);
            request.EnsureSuccessStatusCode();
        }

        public async Task<EntregableDto> DeleteEntregable([FromBody] EntregableDeleteCommand entregable)
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(entregable),
                 Encoding.UTF8,
                 "application/json"
             );

            var request = await _httpClient.PutAsync($"{_apiUrls.AguaUrl}api/agua/entregables/deleteEntregable", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EntregableDto>(
               await request.Content.ReadAsStringAsync(),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
           );
        }

        public async Task AUpdateEntregable([FromForm] EEntregableUpdateCommand entregable)
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(entregable),
                 Encoding.UTF8,
                 "application/json"
             );

            var request = await _httpClient.PutAsync($"{_apiUrls.AguaUrl}api/agua/entregables/autorizarEntregable", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task UpdateEntregableSR([FromBody] ESREntregableUpdateCommand entregable)
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(entregable),
                 Encoding.UTF8,
                 "application/json"
             );

            var request = await _httpClient.PutAsync($"{_apiUrls.AguaUrl}api/agua/entregables/updateEntregableSR", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
