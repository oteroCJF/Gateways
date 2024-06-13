﻿using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Proxies.Comedor.LogCedula.Queries;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.LogCedulas.Queries
{
    public interface IQLCedulaComedorProxy
    {
        Task<List<LogCedulaDto>> GetHistorialByCedula(int cedula);
    }

    public class QLCedulaComedorProxy : IQLCedulaComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QLCedulaComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<LogCedulaDto>> GetHistorialByCedula(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/logCedulas/getHistorialByCedula/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<LogCedulaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
