﻿using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.EntregablesContrato.Queries
{
    public interface IQEContratoComedorProxy
    {
        Task<List<EContratoDto>> GetEntregableContratacionByContrato(int contrato);
        Task<List<EContratoDto>> GetEntregableContratacionByContratoConvenio(int contrato, int convenio);
        Task<EContratoDto> GetEntregableById(int entregable);
        Task<string> VisualizarEntregablesCont(string contrato, string tipoEntregable, string archivo);
        Task<string> VisualizarEntregablesConv(string contrato, string convenio, string tipoEntregable, string archivo);
    }

    public class QEContratoComedorProxy : IQEContratoComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QEContratoComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<EContratoDto>> GetEntregableContratacionByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/entregablesContrato/getEntregablesByContrato/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<List<EContratoDto>> GetEntregableContratacionByContratoConvenio(int contrato, int convenio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/entregablesContrato/getEntregablesByContratoConvenio/{contrato}/{convenio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<EContratoDto> GetEntregableById(int entregable)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/entregablesContrato/getEntregableById/{entregable}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<string> VisualizarEntregablesCont(string contrato, string tipoEntregable, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/entregablesContrato/visualizarEntregableCont/{contrato}/{tipoEntregable}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;
        }
        public async Task<string> VisualizarEntregablesConv(string contrato, string convenio, string tipoEntregable, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/entregablesContrato/visualizarEntregableConv/{contrato}/{convenio}/{tipoEntregable}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}