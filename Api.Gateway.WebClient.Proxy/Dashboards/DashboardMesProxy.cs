using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Dashboard.Cedulas;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Dashboards
{
    public interface IDashboardMesProxy
    {
        Task<List<CedulaDto>> GetDDetalleServicios(int anio, string usuario, int estatus, CTServicioDto servicios);
    }

    public class DashboardMesProxy : IDashboardMesProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public DashboardMesProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CedulaDto>> GetDDetalleServicios(int anio, string usuario, int estatus, CTServicioDto servicios)
        {
            List<CedulaDto> dashboard = new List<CedulaDto>();

            if (servicios.Abreviacion.Equals("Mensajeria"))
            {
                dashboard = await GetDDetalleMensajeria(anio, servicios.Id, estatus, usuario);
            }
            else if (servicios.Abreviacion.Equals("Fumigacion"))
            {
                dashboard = await GetDDetalleFumigacion(anio, servicios.Id, estatus, usuario);
            }

            return dashboard;
        }
        
        public async Task<List<CedulaDto>> GetDDetalleMensajeria(int anio, int servicio, int estatus, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/dashboard/detalle/{estatus}/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<CedulaDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new List<CedulaDto>();
            }
        }
        
        public async Task<List<CedulaDto>> GetDDetalleFumigacion(int anio, int servicio, int estatus, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/dashboard/detalle/{estatus}/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<CedulaDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch(HttpRequestException e)
            {
                return new List<CedulaDto>();
            }
        }
    }
}
