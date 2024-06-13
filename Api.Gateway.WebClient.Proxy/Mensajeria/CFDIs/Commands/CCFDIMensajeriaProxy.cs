using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies.Mensajeria.CFDIs.Commands;
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

namespace Api.Gateway.WebClient.Proxy.Mensajeria.CFDIs.Commands
{
    public interface ICCFDIMensajeriaProxy
    {
        Task<CFDIDto> CreateFactura([FromForm] CFDICreateCommand factura);
        Task<CFDIDto> UpdateFactura([FromForm] CFDIUpdateCommand factura);
        Task CreateHistorialMF([FromBody] HistorialMFCreateCommand command);
    }
    public class CCFDIMensajeriaProxy : ICCFDIMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CCFDIMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(70);
            _apiGatewayUrl = apiGatewayUrl.Value;
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

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/cfdi/createFactura", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CFDIDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CFDIDto> UpdateFactura([FromForm] CFDIUpdateCommand factura)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(factura.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(factura.Mes.ToString()), "Mes");
            formContent.Add(new StringContent(factura.Inmueble.ToString()), "Inmueble");
            var fileStreamContent = new StreamContent(factura.PDF.OpenReadStream());
            fileStreamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(factura.PDF.ContentType);
            formContent.Add(fileStreamContent, name: "PDF", factura.PDF.FileName);

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}mensajeria/cfdi/updateFactura", formContent);
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

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/cfdi/createHistorialMF", content);
            request.EnsureSuccessStatusCode();

        }
    }
}
