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

namespace Api.Gateway.WebClient.Proxy.BMuebles.Solicitudes.Queries
{
    public interface IQBMSolicitudProxy
    {
        Task<List<SolicitudDto>> GetAllSolicitudes();
        Task<SolicitudDto> GetSolicitudById(int id);
    }
    public class QBMSolicitudProxy : IQBMSolicitudProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QBMSolicitudProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<SolicitudDto>> GetAllSolicitudes()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}bmuebles/solicitudes");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<SolicitudDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<SolicitudDto> GetSolicitudById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}bmuebles/solicitudes/getSolicitudById/{id}");
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
