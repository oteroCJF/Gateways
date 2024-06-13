using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies.Comedor.CFDIs.Commands;
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

namespace Api.Gateway.Proxies.Comedor.CFDIs.Queries
{
    public interface IQCFDIComedorProxy
    {
        Task<List<CFDIDto>> GetAllFacturas();
        Task<List<CFDIDto>> GetAllFacturasAsync(int facturacion);
        Task<List<CFDIDto>> GetFacturasByInmuebleAsync(int inmueble, int facturacion);
        Task<List<ConceptoCFDIDto>> GetConceptosFacturaByIdAsync(int factura);
        Task<List<HistorialMFDto>> GetHistorialMFByFacturacion(int facturacion);
        Task<List<CFDIDto>> GetFacturasNCPendientes(int estatus);
        Task<int> GetFacturasCargadasAsync(int facturacion);
        Task<int> GetNotasCreditoCargadasAsync(int facturacion);
        Task<int> GetTotalFacturasByInmuebleAsync(int facturacion, int inmueble);
        Task<int> GetNCByInmuebleAsync(int facturacion, int inmueble);
        Task<decimal> GetFacturacionByCedula(int cedulaId);
        Task<string> VisualizarFactura(int anio, string mes, string folio, string tipo, string inmueble, string archivo);
    }

    public class QCFDIComedorProxy : IQCFDIComedorProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QCFDIComedorProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CFDIDto>> GetAllFacturas()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getAllFacturas");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getFacturasByFacturacion/" + anio);
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getFacturasByInmueble/{inmueble}/{facturacion}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getConceptosFacturaByIdAsync/{factura}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ConceptoCFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<HistorialMFDto>> GetHistorialMFByFacturacion(int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getHistorialMFByFacturacion/{facturacion}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getFacturasNCPendientes/{estatus}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getFacturasCargadas/{facturacion}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getNotasCreditoCargadas/{facturacion}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getTotalFacturasByInmueble/{inmueble}/{facturacion}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getNCByInmueble/{inmueble}/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<decimal> GetFacturacionByCedula(int cedulaId)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/getFacturacionByCedula/{cedulaId}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<decimal>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<string> VisualizarFactura(int anio, string mes, string folio, string tipo, string inmueble, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cfdi/visualizarFactura/{anio}/{mes}/{folio}/{tipo}/{inmueble}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}

