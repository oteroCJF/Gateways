using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Dashboard.Cedulas;
using Api.Gateway.Models.Dashboard.Financieros;
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
    public interface IDFinancierosProxy
    {
        Task<List<DFinancierosDto>> GetDashboardsServicios(int anio, string usuario, List<CTServicioDto> servicios);
    }

    public class DFinancierosProxy : IDFinancierosProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public DFinancierosProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<DFinancierosDto>> GetDashboardsServicios(int anio, string usuario, List<CTServicioDto> servicios)
        {
            List<DFinancierosDto> dashboard = new List<DFinancierosDto>();

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
        
        public async Task<DFinancierosDto> GetDashboardMensajeria(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/dfinancieros/index/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DFinancierosDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new DFinancierosDto();
            }
        }
        
        public async Task<DFinancierosDto> GetDashboardFumigacion(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/dfinancieros/index/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DFinancierosDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch(HttpRequestException e)
            {
                return new DFinancierosDto();
            }
        }
        
        public async Task<DFinancierosDto> GetDashboardLimpieza(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/dfinancieros/index/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DFinancierosDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch(HttpRequestException e)
            {
                return new DFinancierosDto();
            }
        }

        public async Task<DFinancierosDto> GetDashboardComedor(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/dfinancieros/index/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DFinancierosDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new DFinancierosDto();
            }
        }
    }
}
