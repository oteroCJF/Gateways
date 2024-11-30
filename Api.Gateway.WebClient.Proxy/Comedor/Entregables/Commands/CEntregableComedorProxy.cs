using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Models.Estatus.DTOs.EntregablesEstatus;
using Api.Gateway.Proxies.Comedor.Entregables.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.Entregables.Commands
{
    public interface ICEntregableComedorProxy
    {

        Task UpdateEntregable(EntregableCommandUpdate entregable);        
        Task ValidarEntregables([FromForm] EntregableCommandUpdate entregable);        
        Task AUpdateEntregable([FromForm] EEntregableUpdateCommand entregable);
        Task<string> DescargarEntregables([FromBody] DEntregablesCommand command);

    }

    public class CEntregableComedorProxy : ICEntregableComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CEntregableComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task UpdateEntregable(EntregableCommandUpdate entregable)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(entregable.Id.ToString()), "Id");
            formContent.Add(new StringContent(entregable.EstatusId.ToString()), "EstatusId");
            if (entregable.Archivo != null)
            {
                formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");
                var fileStreamContentPDF = new StreamContent(entregable.Archivo.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(entregable.Archivo.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Archivo", entregable.Archivo.FileName);
                formContent.Add(new StringContent(entregable.Anio.ToString()), "Anio");
                formContent.Add(new StringContent(entregable.TipoEntregable.ToString()), "TipoEntregable");
                formContent.Add(new StringContent(entregable.Mes.ToString()), "Mes");
                formContent.Add(new StringContent(entregable.Folio.ToString()), "Folio");
                formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");
            }

            if (entregable.Validar)
            {
                formContent.Add(new StringContent(entregable.Validado.ToString()), "Validado");
                formContent.Add(new StringContent(entregable.Validar.ToString()), "Validar");
            }

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}comedor/entregablesCedula/actualizarEntregable", formContent);
            request.EnsureSuccessStatusCode();
        }

       
        public async Task ValidarEntregables(EntregableCommandUpdate entregable)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(entregable.Id.ToString()), "Id");
            formContent.Add(new StringContent(entregable.EstatusId.ToString()), "EstatusId");
            formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");

            if (entregable.Validar)
            {
                formContent.Add(new StringContent(entregable.Validado.ToString()), "Validado");
                formContent.Add(new StringContent(entregable.Validar.ToString()), "Validar");
            }

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}comedor/entregablesCedula/validarEntregable", formContent);
            request.EnsureSuccessStatusCode();
        }

         public async Task AUpdateEntregable([FromForm] EEntregableUpdateCommand entregable)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(entregable.Id.ToString()), "Id");
            formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(entregable.CedulaEvaluacionId.ToString()), "CedulaEvaluacionId");
            formContent.Add(new StringContent(entregable.EntregableId.ToString()), "EntregableId");
            formContent.Add(new StringContent(entregable.Estatus.ToString()), "Estatus");
            formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}comedor/entregablesCedula/AREntregable", formContent);
            request.EnsureSuccessStatusCode();
        }
        public async Task<string> DescargarEntregables([FromBody] DEntregablesCommand command)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(command),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}comedor/entregablesCedula/descargarEntregables", content);
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;
        }
    }
}
