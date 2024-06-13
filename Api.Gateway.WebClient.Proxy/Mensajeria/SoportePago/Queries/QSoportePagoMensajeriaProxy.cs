using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models;
using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Mensajeria.Commands;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Mensajeria.SoportePago.Queries
{
    public interface IQSoportePagoMensajeriaProxy
    {
        Task<List<MSoportePagoDto>> GetSoportePagoByCedula(int cedula);
        Task<List<MSoportePagoDto>> GetGuiasPendientes(int cedula);
        Task<MSoportePagoDto> GetSoportePagoById(int soporte);
        Task<int> CreateIncidenciaExcel([FromForm] MSoportePagoCreateCommand command);
        Task<int> ActualizaIncidenciaExcel([FromForm] MSoportePagoUpdateCommand command);
    }

    public class QSoportePagoMensajeriaProxy : IQSoportePagoMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QSoportePagoMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<MSoportePagoDto>> GetSoportePagoByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/soportePago/getSoportePagoByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<MSoportePagoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<MSoportePagoDto>> GetGuiasPendientes(int cedula)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/soportePago/getGuiasPendientes/{cedula}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<MSoportePagoDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new List<MSoportePagoDto>();
            }
        }

        public async Task<MSoportePagoDto> GetSoportePagoById(int soporte)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/soportePago/getSoportePagoById/{soporte}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MSoportePagoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> CreateIncidenciaExcel([FromForm] MSoportePagoCreateCommand incidencia)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(incidencia.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(incidencia.UsuarioId != null ? incidencia.UsuarioId.ToString() : ""), "UsuarioId");
            formContent.Add(new StringContent(incidencia.Folio.ToString()), "Folio");
            formContent.Add(new StringContent(incidencia.Mes.ToString()), "Mes");
            formContent.Add(new StringContent(incidencia.MesId.ToString()), "MesId");

            var fileStreamContentXLSX = new StreamContent(incidencia.TXT.OpenReadStream());
            fileStreamContentXLSX.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.TXT.ContentType);
            formContent.Add(fileStreamContentXLSX, name: "TXT", incidencia.TXT.FileName);

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/soportePago/insertaSoportePago", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> ActualizaIncidenciaExcel([FromForm] MSoportePagoUpdateCommand incidencia)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(incidencia.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(incidencia.UsuarioId != null ? incidencia.UsuarioId.ToString() : ""), "UsuarioId");
            formContent.Add(new StringContent(incidencia.Folio.ToString()), "Folio");
            formContent.Add(new StringContent(incidencia.Mes.ToString()), "Mes");
            formContent.Add(new StringContent(incidencia.MesId.ToString()), "MesId");

            var fileStreamContentXLSX = new StreamContent(incidencia.TXT.OpenReadStream());
            fileStreamContentXLSX.Headers.ContentType = MediaTypeHeaderValue.Parse(incidencia.TXT.ContentType);
            formContent.Add(fileStreamContentXLSX, name: "TXT", incidencia.TXT.FileName);

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/soportePago/actualizaSoportePago", formContent);
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
