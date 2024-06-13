using Api.Gateway.Models.Incidencias.Mensajeria.Commands;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Mensajeria.SoportePago.Commands
{
    public interface ICSoportePagoMensajeriaProxy
    {
        Task<int> CreateSoportePago([FromForm] MSoportePagoCreateCommand command);
        Task<int> ActualizaSoportePago([FromForm] MSoportePagoUpdateCommand command);
    }

    public class CSoportePagoMensajeriaProxy : ICSoportePagoMensajeriaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CSoportePagoMensajeriaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<int> CreateSoportePago([FromForm] MSoportePagoCreateCommand incidencia)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(incidencia.Cedulas);
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(incidencia.Folio.ToString()), "Folio");
            formContent.Add(new StringContent(incidencia.Mes.ToString()), "Mes");
            formContent.Add(new StringContent(incidencia.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(incidencia.UsuarioId != null ? incidencia.UsuarioId.ToString() : ""), "UsuarioId");
            formContent.Add(new StringContent(json), "Cedulas");

            var fileStreamContentXLSX = new StreamContent(incidencia.TXT.OpenReadStream());
            fileStreamContentXLSX.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.TXT.ContentType);
            formContent.Add(fileStreamContentXLSX, name: "TXT", incidencia.TXT.FileName);

            var request = await _httpClient.PostAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/soportePago/insertaSoportePago", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> ActualizaSoportePago([FromForm] MSoportePagoUpdateCommand incidencia)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(incidencia.Cedulas);
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(incidencia.Folio.ToString()), "Folio");
            formContent.Add(new StringContent(incidencia.Mes.ToString()), "Mes");
            formContent.Add(new StringContent(incidencia.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(incidencia.UsuarioId != null ? incidencia.UsuarioId.ToString() : ""), "UsuarioId");
            formContent.Add(new StringContent(json), "Cedulas");

            var fileStreamContentXLSX = new StreamContent(incidencia.TXT.OpenReadStream());
            fileStreamContentXLSX.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.TXT.ContentType);
            formContent.Add(fileStreamContentXLSX, name: "TXT", incidencia.TXT.FileName);

            var request = await _httpClient.PostAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/soportePago/actualizaSoportePago", formContent);
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
