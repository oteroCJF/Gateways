using Api.Gateway.Models.CFDIs.ServiciosBasicos.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.ServiciosBasicos.AEElectrica.CFDIs
{
    public interface IAEECFDIProxy
    {
        Task<List<CFDIDto>> GetFacturasBySolicitudAsync(int solicitud);
        Task<int> CreateFactura([FromForm] CFDISBCreateCommand factura);
        Task<int> DeleteFactura([FromBody] CFDISBDeleteCommand factura);
    }
    public class AEECFDIProxy : IAEECFDIProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public AEECFDIProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CFDIDto>> GetFacturasBySolicitudAsync(int solicitud)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/cfdi/getCFDIBySolicitud/{solicitud}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> CreateFactura([FromForm] CFDISBCreateCommand factura)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(factura.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(factura.SolicitudId.ToString()), "SolicitudId");
            formContent.Add(new StringContent(factura.Mes.ToString()), "Mes");
            formContent.Add(new StringContent(factura.UsuarioId.ToString()), "UsuarioId");
            var fileXMLContent = new StreamContent(factura.XML.OpenReadStream());
            fileXMLContent.Headers.ContentType = MediaTypeHeaderValue.Parse(factura.XML.ContentType);
            formContent.Add(fileXMLContent, name: "XML", factura.XML.FileName);
            
            var filePDFContent = new StreamContent(factura.PDF.OpenReadStream());
            filePDFContent.Headers.ContentType = MediaTypeHeaderValue.Parse(factura.PDF.ContentType);
            formContent.Add(filePDFContent, name: "PDF", factura.PDF.FileName);

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}aeelectrica/cfdi/createCFDI", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> DeleteFactura([FromBody] CFDISBDeleteCommand factura)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(factura),
                  Encoding.UTF8,
                  "application/json"
              );
            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}aeelectrica/cfdi/deleteCFDI", content);
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
