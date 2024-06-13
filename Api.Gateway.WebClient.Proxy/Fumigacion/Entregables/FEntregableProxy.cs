using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Models.Estatus.DTOs.EntregablesEstatus;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Fumigacion.Entregables
{
    public interface IFEntregableProxy
    {
        Task<List<EntregableDto>> GetEntregablesByCedula(int cedula);
        Task<List<EntregableEstatusDto>> GetEntregablesByEstatus(int estatus);
        Task UpdateEntregable(EntregableCommandUpdate entregable);
        Task AUpdateEntregable([FromForm] EEntregableUpdateCommand entregable);
        Task<string> VisualizarEntregables(int anio, string mes, string folio, string archivo, string tipo);

        Task<string> DescargarEntregables([FromBody] DEntregablesCommand command);
    }

    public class FEntregableProxy : IFEntregableProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public FEntregableProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<EntregableEstatusDto>> GetEntregablesByEstatus(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/entregablesCedula/getEntregablesByEstatus/{estatus}");
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
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/entregablesCedula/getEntregablesByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EntregableDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
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

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}fumigacion/entregablesCedula/actualizarEntregable", formContent);
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

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}fumigacion/entregablesCedula/AREntregable", formContent);
            request.EnsureSuccessStatusCode();
        }

        public async Task<string> VisualizarEntregables(int anio, string mes, string folio, string archivo, string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/entregablesCedula/visualizarEntregable/{anio}/{mes}/{folio}/{archivo}/{tipo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;
        }

        public async Task<string> DescargarEntregables([FromBody] DEntregablesCommand command)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(command),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}fumigacion/entregablesCedula/descargarEntregables", content);
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;
        }
    }
}
