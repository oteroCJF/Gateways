using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Dashboard.Financieros;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Agua.CFDIs.Commands
{
    public interface ICCFDIAguaProxy
    {
        Task<CFDIDto> CreateFactura([FromForm] CFDICreateCommand command);
        Task<CFDIDto> UpdateFactura([FromForm] CFDIUpdateCommand command);
        Task CreateHistorialMF([FromBody] HistorialMFCreateCommand command);
    }

    public class CCFDIAguaProxy : ICCFDIAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CCFDIAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            _apiUrls = apiUrls.Value;
        }

        public async Task<CFDIDto> CreateFactura([FromForm] CFDICreateCommand factura)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(factura.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(factura.InmuebleId.ToString()), "InmuebleId");
            formContent.Add(new StringContent(factura.RepositorioId.ToString()), "RepositorioId");
            formContent.Add(new StringContent(factura.Mes.ToString()), "Mes");
            formContent.Add(new StringContent(factura.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(factura.Inmueble.ToString()), "Inmueble");
            formContent.Add(new StringContent(factura.TipoFacturacion.ToString()), "TipoFacturacion");
            var fileXMLContent = new StreamContent(factura.XML.OpenReadStream());
            fileXMLContent.Headers.ContentType = MediaTypeHeaderValue.Parse(factura.XML.ContentType);
            formContent.Add(fileXMLContent, name: "XML", factura.XML.FileName);
            if (factura.PDF != null)
            {
                var filePDFContent = new StreamContent(factura.PDF.OpenReadStream());
                filePDFContent.Headers.ContentType = MediaTypeHeaderValue.Parse(factura.PDF.ContentType);
                formContent.Add(filePDFContent, name: "PDF", factura.PDF.FileName);
            }

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/cfdi/createFactura", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CFDIDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CFDIDto> UpdateFactura([FromForm] CFDIUpdateCommand command)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(command.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(command.Mes.ToString()), "Mes");
            formContent.Add(new StringContent(command.Inmueble.ToString()), "Inmueble");

            var fileStreamContentPDF = new StreamContent(command.PDF.OpenReadStream());
            fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(command.PDF.ContentType);
            formContent.Add(fileStreamContentPDF, name: "PDF", command.PDF.FileName);

            //content.Headers.

            var request = await _httpClient.PutAsync($"{_apiUrls.AguaUrl}api/agua/cfdi/updateFactura", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CFDIDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task CreateHistorialMF([FromBody] HistorialMFCreateCommand command)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/cfdi/createHistorialMF", content);
            request.EnsureSuccessStatusCode();

        }
    }
}
