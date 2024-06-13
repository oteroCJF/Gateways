using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.BMuebles.EntregablesContrato.Commands
{
    public interface ICBMEContratoProxy
    {
        Task<int> UpdateEntregable([FromForm] EntregableContratoUpdateCommand entregable);
    }

    public class CBMEContratoProxy : ICBMEContratoProxy
    {

        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CBMEContratoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
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

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}bmuebles/entregablesContrato/updateEntregableContratacion", formContent);
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
