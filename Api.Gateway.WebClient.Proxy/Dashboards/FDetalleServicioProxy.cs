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
    public interface IFDetalleServicioProxy
    {
        Task<List<DetalleServicioDto>> GetFDetalleServicios(int anio, string usuario, CTServicioDto servicio);
    }

    public class FDetalleServicioProxy : IFDetalleServicioProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public FDetalleServicioProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<DetalleServicioDto>> GetFDetalleServicios(int anio, string usuario, CTServicioDto servicio)
        {
            if (servicio.Abreviacion.Equals("Mensajeria"))
            {
                return await GetDetalleFM(anio, servicio.Id, usuario);
            }
            else if (servicio.Abreviacion.Equals("Fumigacion"))
            {
                return await GetDetalleFF(anio, servicio.Id, usuario);
            }
            else if (servicio.Abreviacion.Equals("Limpieza"))
            {
                return await GetDetalleFL(anio, servicio.Id, usuario);
            }
            else if (servicio.Abreviacion.Equals("Agua"))
            {
                return await GetDetalleFA(anio, servicio.Id, usuario);
            }
            else if (servicio.Abreviacion.Equals("Comedor"))
            {
                return await GetDetalleFC(anio, servicio.Id, usuario);
            }
            return new List<DetalleServicioDto>();
        }


        public async Task<List<DetalleServicioDto>> GetDetalleFM(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/dfinancieros/detalle/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<DetalleServicioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new List<DetalleServicioDto>();
            }
        }

        public async Task<List<DetalleServicioDto>> GetDetalleFF(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/dfinancieros/detalle/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<DetalleServicioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new List<DetalleServicioDto>();
            }
        }

        public async Task<List<DetalleServicioDto>> GetDetalleFL(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/dfinancieros/detalle/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<DetalleServicioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new List<DetalleServicioDto>();
            }
        }

        public async Task<List<DetalleServicioDto>> GetDetalleFA(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/dfinancieros/detalle/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<DetalleServicioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new List<DetalleServicioDto>();
            }

        }

        public async Task<List<DetalleServicioDto>> GetDetalleFC(int anio, int servicio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/dfinancieros/detalle/{anio}/{servicio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<DetalleServicioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new List<DetalleServicioDto>();
            }
        }
    }
}

