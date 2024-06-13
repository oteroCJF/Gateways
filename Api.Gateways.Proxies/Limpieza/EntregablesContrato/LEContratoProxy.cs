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
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using System;

namespace Api.Gateway.Proxies.Limpieza.EntregablesContratacion
{
    public interface ILEContratoProxy
    {
        Task<List<EContratoDto>> GetEntregableContratacionByContrato(int contrato);
        Task<List<EContratoDto>> GetEntregableContratacionByContratoConvenio(int contrato, int convenio);
        Task<EContratoDto> GetEntregableById(int entregable);
        Task<int> UpdateEntregable([FromForm] EntregableContratoUpdateCommand command);
        Task<string> VisualizarEntregablesCont(string contrato, string tipoEntregable, string archivo);
        Task<string> VisualizarEntregablesConv(string contrato, string convenio, string tipoEntregable, string archivo);
    }
    public class LEContratoProxy : ILEContratoProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public LEContratoProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            _apiUrls = apiUrls.Value;
        }
        public async Task<List<EContratoDto>> GetEntregableContratacionByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregablesContratacion/getEntregablesByContrato/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<List<EContratoDto>> GetEntregableContratacionByContratoConvenio(int contrato, int convenio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregablesContratacion/getEntregablesByContratoConvenio/{contrato}/{convenio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<EContratoDto> GetEntregableById(int entregable)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregablesContratacion/getEntregableById/{entregable}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<int> UpdateEntregable([FromForm] EntregableContratoUpdateCommand entregable)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(entregable.Id.ToString()), "Id");
            formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(entregable.EntregableId.ToString()), "EntregableId");
            formContent.Add(new StringContent(entregable.FechaProgramada.ToString()), "FechaProgramada");
            formContent.Add(new StringContent(entregable.FechaEntrega.ToString()), "FechaEntrega");
            formContent.Add(new StringContent(entregable.InicioVigencia.ToString()), "InicioVigencia");
            formContent.Add(new StringContent(entregable.FinVigencia.ToString()), "FinVigencia");
            formContent.Add(new StringContent(entregable.MontoGarantia.ToString()), "MontoGarantia");
            formContent.Add(new StringContent(entregable.Penalizable.ToString()), "Penalizable");
            formContent.Add(new StringContent(entregable.MontoPenalizacion.ToString()), "MontoPenalizacion");
            formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");

            if (entregable.Archivo != null)
            {
                formContent.Add(new StringContent(entregable.Contrato.ToString()), "Contrato");
                formContent.Add(new StringContent(entregable.Convenio.ToString()), "Convenio");
                formContent.Add(new StringContent(entregable.TipoEntregable.ToString()), "TipoEntregable");
                var fileStreamContentPDF = new StreamContent(entregable.Archivo.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(entregable.Archivo.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Archivo", entregable.Archivo.FileName);
            }

            var request = await _httpClient.PutAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregablesContratacion/updateEntregableContratacion", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
               await request.Content.ReadAsStringAsync(),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
           );
        }
        public async Task<string> VisualizarEntregablesCont(string contrato, string tipoEntregable, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregablesContratacion/visualizarEntregableCont/{contrato}/{tipoEntregable}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;
        }
        public async Task<string> VisualizarEntregablesConv(string contrato, string convenio, string tipoEntregable, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/entregablesContratacion/visualizarEntregableConv/{contrato}/{convenio}/{tipoEntregable}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
