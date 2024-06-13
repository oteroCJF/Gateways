using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Dashboard.Cedulas;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Oficios.Commands;
using Api.Gateway.Models.Oficios.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Agua.Oficios.Queries
{
    public interface IQOficioAguaProxy
    {
        Task<List<OficioDto>> GetAllOficiosAsync();
        Task<List<OficioDto>> GetOficiosByAnio(int anio);
        Task<OficioDto> GetOficioById(int id);
        Task<List<CFDIDto>> GetFacturasNCPendientes(int oficio);
    }

    public class QOficioAguaProxy : IQOficioAguaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QOficioAguaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<OficioDto>> GetAllOficiosAsync()
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/oficios");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<OficioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException ex)
            {
                return new List<OficioDto>();
            }
        }

        public async Task<List<OficioDto>> GetOficiosByAnio(int anio)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/oficios/getOficiosByAnio/{anio}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<OficioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException ex)
            {
                return new List<OficioDto>();
            }
        }

        public async Task<OficioDto> GetOficioById(int id)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/oficios/getOficioById/{id}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<OficioDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException ex)
            {
                return new OficioDto();
            }
        }

        public async Task<List<CFDIDto>> GetFacturasNCPendientes(int oficio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/oficios/getFacturasNCPendientes/{oficio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
