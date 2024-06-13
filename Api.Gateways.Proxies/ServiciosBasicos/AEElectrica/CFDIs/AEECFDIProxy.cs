using Api.Gateway.Models.CFDIs.ServiciosBasicos.Commands;
using Api.Gateway.Models.CFDIs.ServiciosBasicos.DTOs;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.CFDIs
{
    public interface IAEECFDIProxy
    {
        Task<List<CFDISBDto>> GetFacturasBySolicitudAsync(int solicitud);
        Task<List<ConceptoCFDISBDto>> GetConceptosFacturaByIdAsync(int factura);
        Task<List<ASGeneralesDto>> GetASGeneralesByIdAsync(int factura);
        Task<int> CreateFactura([FromForm] CFDISBCreateCommand command);
        Task<int> DeleteFactura([FromBody] CFDISBDeleteCommand command);
    }

    public class AEECFDIProxy : IAEECFDIProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public AEECFDIProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CFDISBDto>> GetFacturasBySolicitudAsync(int solicitud)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/cfdi/getCFDIBySolicitud/{solicitud}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDISBDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<ConceptoCFDISBDto>> GetConceptosFacturaByIdAsync(int factura)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/cfdi/getConceptosByCFDI/{factura}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ConceptoCFDISBDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<ASGeneralesDto>> GetASGeneralesByIdAsync(int factura)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/cfdi/getASGeneralesByCFDI/{factura}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ASGeneralesDto>>(
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
            formContent.Add(new StringContent(factura.Mes.ToString()), "Mes");
            formContent.Add(new StringContent(factura.SolicitudId.ToString()), "SolicitudId");
            formContent.Add(new StringContent(factura.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(factura.EntregableId.ToString()), "EntregableId");
            
            var fileXMLContent = new StreamContent(factura.XML.OpenReadStream());
            fileXMLContent.Headers.ContentType = MediaTypeHeaderValue.Parse(factura.XML.ContentType);
            formContent.Add(fileXMLContent, name: "XML", factura.XML.FileName);

            var filePDFContent = new StreamContent(factura.PDF.OpenReadStream());
            filePDFContent.Headers.ContentType = MediaTypeHeaderValue.Parse(factura.PDF.ContentType);
            formContent.Add(filePDFContent, name: "PDF", factura.PDF.FileName);

            var request = await _httpClient.PostAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/cfdi/createCFDI", formContent);
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

            var request = await _httpClient.PutAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/cfdi/deleteCFDI", content);
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
