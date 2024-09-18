﻿using Api.Gateway.Models.Catalogos.DTOs.Servicios;
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

namespace Api.Gateway.WebClient.Proxy.Comedor.Oficios.Queries
{
    public interface IQueriesOficiosProxy
    {
        Task<List<OficioDto>> GetAllOficiosAsync();
        Task<List<OficioDto>> GetOficiosByAnio(int anio);
        Task<OficioDto> GetOficioById(int id);
        Task<List<CFDIDto>> GetFacturasNCPendientes(int oficio);

    }

    public class QOficioComedorProxy : IQueriesOficiosProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QOficioComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<OficioDto>> GetAllOficiosAsync()
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/oficios");
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
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/oficios/getOficiosByAnio/{anio}");
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
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/oficios/getOficioById/{id}");
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
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/oficios/getFacturasNCPendientes/{oficio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<OficioDto> CreateOficio([FromForm] OficioCreateCommand oficio)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(oficio.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(oficio.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(oficio.NumeroOficio.ToString()), "NumeroOficio");
            formContent.Add(new StringContent(oficio.ContratoId.ToString()), "ContratoId");
            formContent.Add(new StringContent(oficio.ServicioId.ToString()), "ServicioId");
            formContent.Add(new StringContent(oficio.FechaTramitado.ToString()), "FechaTramitado");
            if (oficio.Oficio != null)
            {
                var oficioContent = new StreamContent(oficio.Oficio.OpenReadStream());
                oficioContent.Headers.ContentType = MediaTypeHeaderValue.Parse(oficio.Oficio.ContentType);
                formContent.Add(oficioContent, name: "Oficio", oficio.Oficio.FileName);
            }

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}comedor/oficios/createOficio", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}