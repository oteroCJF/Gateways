﻿using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Fumigacion.CedulaEvaluacion
{
    public interface IFRespuestasProxy
    {
        Task UpdateRespuestas(List<RespuestasUpdateCommand> respuestas);
    }

    public class FRespuestasProxy : IFRespuestasProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public FRespuestasProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task UpdateRespuestas(List<RespuestasUpdateCommand> respuestas)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(respuestas),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}fumigacion/cedulaEvaluacion/updateRespuestasByCedula", content);
            request.EnsureSuccessStatusCode();
        }
    }
}
