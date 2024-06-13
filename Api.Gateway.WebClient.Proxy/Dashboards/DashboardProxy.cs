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
    public interface IDashboardProxy
    {
        Task<List<DashboardDto>> GetDashboardsServicios(int anio, string usuario, List<CTServicioDto> servicios);
    }

    public class DashboardProxy : IDashboardProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public DashboardProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<DashboardDto>> GetDashboardsServicios(int anio, string usuario, List<CTServicioDto> servicios)
        {
            List<DashboardDto> dashboard = new List<DashboardDto>();

            foreach (var sc in servicios)
            {
                if (sc.Abreviacion.Equals("Mensajeria"))
                {
                    dashboard.Add(await GetDashboardMensajeria(anio, sc.Id, usuario));
                }
                
                if (sc.Abreviacion.Equals("Fumigacion"))
                {
                    dashboard.Add(await GetDashboardFumigacion(anio, sc.Id, usuario));
                }
                
                if (sc.Abreviacion.Equals("Limpieza"))
                {
                    dashboard.Add(await GetDashboardLimpieza(anio, sc.Id, usuario));
                }
                
                if (sc.Abreviacion.Equals("Comedor"))
                {
                    dashboard.Add(await GetDashboardComedor(anio, sc.Id, usuario));
                }
            }

            return dashboard;
        }
        
        public async Task<DashboardDto> GetDashboardMensajeria(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/dashboard/index/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DashboardDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new DashboardDto();
            }
        }
        
        public async Task<DashboardDto> GetDashboardLimpieza(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/dashboard/index/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DashboardDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new DashboardDto();
            }
        }
        
        public async Task<DashboardDto> GetDashboardFumigacion(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/dashboard/index/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DashboardDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch(HttpRequestException e)
            {
                return new DashboardDto();
            }
        }
        
        public async Task<DashboardDto> GetDashboardComedor(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/dashboard/index/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DashboardDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch(HttpRequestException e)
            {
                return new DashboardDto();
            }
        }
    }
}
