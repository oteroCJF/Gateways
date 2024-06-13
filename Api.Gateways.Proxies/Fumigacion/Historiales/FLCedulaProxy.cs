﻿using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Fumigacion.Historiales
{
    public interface IFLCedulaProxy
    {
        Task<List<LogCedulaDto>> GetHistorialByCedula(int cedula);
        Task CreateHistorial([FromBody] LogCedulaCreateCommand historial);
    }

    public class FLCedulaProxy : IFLCedulaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public FLCedulaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<LogCedulaDto>> GetHistorialByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/logCedulas/getHistorialByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LogCedulaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task CreateHistorial([FromBody] LogCedulaCreateCommand historial)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(historial),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/logCedulas/createHistorial", content);
            request.EnsureSuccessStatusCode();
        }
    }
}


