﻿using Api.Gateway.Models.Contratos.Commands.ServicioContrato;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Comedor.ServiciosContrato.Queries;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.ServiciosContrato.Queries
{
    public interface IQSContratoComedorProxy
    {
        Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato);
    }

    public class QSContratoComedorProxy : IQSContratoComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QSContratoComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/servicioContrato/getServiciosContrato/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ServicioContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
