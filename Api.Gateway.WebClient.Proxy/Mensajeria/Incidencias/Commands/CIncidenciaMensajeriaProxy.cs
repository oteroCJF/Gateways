using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Mensajeria.Commands;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using Api.Gateway.Proxies.Mensajeria.Incidencias.Commands;
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

namespace Api.Gateway.WebClient.Proxy.Mensajeria.Incidencias.Commands
{
    public interface ICIncidenciaMensajeriaProxy
    {
        Task CreateIncidencia([FromForm] MIncidenciaCreateCommand command);
        Task<List<MIncidenciaDto>> CreateIncidenciaExcel([FromForm] MIncidenciaExcelCreateCommand command);
        Task UpdateIncidencia([FromForm] MIncidenciaUpdateCommand command);
        Task<int> DeleteIncidencias([FromBody] MIncidenciaDeleteCommand incidencia);
        Task<int> DeleteIncidencia([FromBody] MIncidenciaDeleteCommand incidencia);
    }

    public class CIncidenciaMensajeriaProxy : ICIncidenciaMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CIncidenciaMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }


        public async Task CreateIncidencia([FromForm] MIncidenciaCreateCommand incidencia)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(incidencia.UsuarioId != null ? incidencia.UsuarioId.ToString() : ""), "UsuarioId");
            formContent.Add(new StringContent(incidencia.IncidenciaId.ToString()), "IncidenciaId");
            formContent.Add(new StringContent(incidencia.IndemnizacionId.ToString()), "IndemnizacionId");
            formContent.Add(new StringContent(incidencia.Pregunta.ToString()), "Pregunta");
            formContent.Add(new StringContent(incidencia.CedulaEvaluacionId.ToString()), "CedulaEvaluacionId");
            formContent.Add(new StringContent(incidencia.FechaProgramada != null ? incidencia.FechaProgramada.ToString() : ""), "FechaProgramada");
            formContent.Add(new StringContent(incidencia.FechaEntrega != null ? incidencia.FechaEntrega.ToString() : ""), "FechaEntrega");
            formContent.Add(new StringContent(incidencia.NumeroGuia != null ? incidencia.NumeroGuia.ToString() : ""), "NumeroGuia");
            formContent.Add(new StringContent(incidencia.CodigoRastreo != null ? incidencia.CodigoRastreo.ToString() : ""), "CodigoRastreo");
            formContent.Add(new StringContent(incidencia.Acuse != null ? incidencia.Acuse.ToString() : ""), "Acuse");
            formContent.Add(new StringContent(incidencia.TotalAcuses.ToString()), "TotalAcuses");
            formContent.Add(new StringContent(incidencia.TipoServicio != null ? incidencia.TipoServicio.ToString() : ""), "TipoServicio");
            formContent.Add(new StringContent(incidencia.Sobrepeso.ToString()), "Sobrepeso");
            formContent.Add(new StringContent(incidencia.Observaciones != null ? incidencia.Observaciones.ToString() : ""), "Observaciones");

            if (incidencia.Acta != null)
            {
                var fileStreamContentPDF = new StreamContent(incidencia.Acta.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.Acta.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Acta", incidencia.Acta.FileName);
            }

            if (incidencia.Escrito != null)
            {
                var fileStreamContentPDF = new StreamContent(incidencia.Escrito.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.Escrito.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Escrito", incidencia.Escrito.FileName);
            }

            if (incidencia.Comprobante != null)
            {
                var fileStreamContentPDF = new StreamContent(incidencia.Comprobante.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.Comprobante.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Comprobante", incidencia.Comprobante.FileName);
            }

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/insertaIncidencia", formContent);
            request.EnsureSuccessStatusCode();
        }

        public async Task<List<MIncidenciaDto>> CreateIncidenciaExcel([FromForm] MIncidenciaExcelCreateCommand incidencia)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(incidencia.UsuarioId != null ? incidencia.UsuarioId.ToString() : ""), "UsuarioId");
            formContent.Add(new StringContent(incidencia.IncidenciaId.ToString()), "IncidenciaId");
            formContent.Add(new StringContent(incidencia.Pregunta.ToString()), "Pregunta");
            formContent.Add(new StringContent(incidencia.CedulaEvaluacionId.ToString()), "CedulaEvaluacionId");

            var fileStreamContentXLSX = new StreamContent(incidencia.Excel.OpenReadStream());
            fileStreamContentXLSX.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.Excel.ContentType);
            formContent.Add(fileStreamContentXLSX, name: "Excel", incidencia.Excel.FileName);

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/insertaIncidenciaExcel", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MIncidenciaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task UpdateIncidencia([FromForm] MIncidenciaUpdateCommand incidencia)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(incidencia.Id.ToString()), "Id");
            formContent.Add(new StringContent(incidencia.UsuarioId != null ? incidencia.UsuarioId.ToString() : ""), "UsuarioId");
            formContent.Add(new StringContent(incidencia.IncidenciaId.ToString()), "IncidenciaId");
            formContent.Add(new StringContent(incidencia.IndemnizacionId.ToString()), "IndemnizacionId");
            formContent.Add(new StringContent(incidencia.Pregunta.ToString()), "Pregunta");
            formContent.Add(new StringContent(incidencia.CedulaEvaluacionId.ToString()), "CedulaEvaluacionId");
            formContent.Add(new StringContent(incidencia.FechaProgramada != null ? incidencia.FechaProgramada.ToString() : ""), "FechaProgramada");
            formContent.Add(new StringContent(incidencia.FechaEntrega != null ? incidencia.FechaEntrega.ToString() : ""), "FechaEntrega");
            formContent.Add(new StringContent(incidencia.NumeroGuia != null ? incidencia.NumeroGuia.ToString() : ""), "NumeroGuia");
            formContent.Add(new StringContent(incidencia.CodigoRastreo != null ? incidencia.CodigoRastreo.ToString() : ""), "CodigoRastreo");
            formContent.Add(new StringContent(incidencia.Acuse != null ? incidencia.Acuse.ToString() : ""), "Acuse");
            formContent.Add(new StringContent(incidencia.TotalAcuses.ToString()), "TotalAcuses");
            formContent.Add(new StringContent(incidencia.TipoServicio != null ? incidencia.TipoServicio.ToString() : ""), "TipoServicio");
            formContent.Add(new StringContent(incidencia.Sobrepeso.ToString()), "Sobrepeso");
            formContent.Add(new StringContent(incidencia.Observaciones != null ? incidencia.Observaciones.ToString() : ""), "Observaciones");

            if (incidencia.Acta != null)
            {
                var fileStreamContentPDF = new StreamContent(incidencia.Acta.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.Acta.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Acta", incidencia.Acta.FileName);
            }

            if (incidencia.Escrito != null)
            {
                var fileStreamContentPDF = new StreamContent(incidencia.Escrito.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.Escrito.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Escrito", incidencia.Escrito.FileName);
            }

            if (incidencia.Comprobante != null)
            {
                var fileStreamContentPDF = new StreamContent(incidencia.Comprobante.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.Comprobante.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Comprobante", incidencia.Comprobante.FileName);
            }

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/actualizarIncidencia", formContent);
            request.EnsureSuccessStatusCode();
        }

        public async Task<int> DeleteIncidencias([FromBody] MIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/eliminarIncidencias", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> DeleteIncidencia([FromBody] MIncidenciaDeleteCommand incidencia)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(incidencia),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/incidenciasCedula/eliminarIncidencia", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
