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

namespace Api.Gateway.Proxies.Limpieza.Facturas 
{
    public interface ILCFDIProxy
    {
        Task<List<CFDIDto>> GetAllFacturas();
        Task<List<CFDIDto>> GetAllFacturasAsync(int facturacion);
        Task<List<CFDIDto>> GetFacturasByInmuebleAsync(int inmueble, int facturacion);
        Task<List<ConceptoCFDIDto>> GetConceptosFacturaByIdAsync(int factura);
        Task<List<HistorialMFDto>> GetHistorialMFByRepositorio(int id);
        Task<List<CFDIDto>> GetFacturasNCPendientes(int estatus);

        Task<int> GetFacturasCargadasAsync(int facturacion);
        Task<int> GetNotasCreditoCargadasAsync(int facturacion);
        Task<int> GetTotalFacturasByInmuebleAsync(int facturacion, int inmueble);
        Task<int> GetNCByInmuebleAsync(int facturacion, int inmueble);
        Task<CFDIDto> CreateFactura([FromForm] CFDICreateCommand command);
        Task<CFDIDto> UpdateFactura([FromForm] CFDIUpdateCommand command);
        Task CreateHistorialMF([FromBody] HistorialMFCreateCommand command);
        Task<string> VisualizarFactura(int anio, string mes, string folio, string tipo, string inmueble, string archivo);
    }

    public class LCFDIProxy : ILCFDIProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public LCFDIProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CFDIDto>> GetAllFacturas()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getAllFacturas");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CFDIDto>> GetAllFacturasAsync(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getFacturasByFacturacion/" + anio);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CFDIDto>> GetFacturasByInmuebleAsync(int inmueble, int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getFacturasByInmueble/{inmueble}/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<ConceptoCFDIDto>> GetConceptosFacturaByIdAsync(int factura)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getConceptosFacturaByIdAsync/{factura}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ConceptoCFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<HistorialMFDto>> GetHistorialMFByRepositorio(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getHistorialMFByRepositorio/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<HistorialMFDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CFDIDto>> GetFacturasNCPendientes(int estatus)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getFacturasNCPendientes/{estatus}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<int> GetFacturasCargadasAsync(int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getFacturasCargadas/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> GetNotasCreditoCargadasAsync(int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getNotasCreditoCargadas/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> GetTotalFacturasByInmuebleAsync(int inmueble, int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getTotalFacturasByInmueble/{inmueble}/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> GetNCByInmuebleAsync(int inmueble, int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/getNCByInmueble/{inmueble}/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/createFactura", formContent);
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

            var request = await _httpClient.PutAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/updateFactura", formContent);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/createHistorialMF", content);
            request.EnsureSuccessStatusCode();

        }

        public async Task<string> VisualizarFactura(int anio, string mes, string folio, string tipo, string inmueble, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/cfdi/visualizarFactura/{anio}/{mes}/{folio}/{tipo}/{inmueble}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
