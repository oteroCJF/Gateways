using Api.Gateway.Models.BMuebles.Solicitudes.Commands;
using Api.Gateway.Models.BMuebles.Solicitudes.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.BMuebles.Solicitudes.Commands
{
    public interface ICBMSolicitudProxy
    {
        Task<SolicitudDto> CreateSolicitud([FromBody] SolicitudCreateCommand solicitud);
    }
    public class CBMSolicitudProxy : ICBMSolicitudProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CBMSolicitudProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<SolicitudDto> CreateSolicitud([FromBody] SolicitudCreateCommand solicitud)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(solicitud),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}bmuebles/solicitudes/createSolicitud", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<SolicitudDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
