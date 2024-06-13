using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
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

namespace Api.Gateway.WebClient.Proxy.Mensajeria.CFDIs.Queries
{
    public interface IQCFDIMensajeriaProxy
    {
        Task<List<CFDIDto>> GetAllFacturas();
        Task<List<CFDIDto>> GetAllFacturasByRepositorio(int repositorio);
        Task<List<CFDIDto>> GetFacturasByInmueble(int inmueble, int facturacion);
        Task<List<HistorialMFDto>> GetHistorialMFByFacturacion(int facturacion);
        Task<string> VisualizarFactura(int anio, string mes, string folio, string tipo, string inmueble, string archivo);
    }
    public class QCFDIMensajeriaProxy : IQCFDIMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QCFDIMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(70);
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CFDIDto>> GetAllFacturas()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cfdi/getAllFacturas");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CFDIDto>> GetAllFacturasByRepositorio(int repositorio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cfdi/getFacturasByRepositorio/{repositorio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CFDIDto>> GetFacturasByInmueble(int inmueble, int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cfdi/getFacturasByInmueble/{inmueble}/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<HistorialMFDto>> GetHistorialMFByFacturacion(int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cfdi/getHistorialMFByFacturacion/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<HistorialMFDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<string> VisualizarFactura(int anio, string mes, string folio, string tipo, string inmueble, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cfdi/visualizarFactura/{anio}/{mes}/{folio}/{tipo}/{inmueble}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
