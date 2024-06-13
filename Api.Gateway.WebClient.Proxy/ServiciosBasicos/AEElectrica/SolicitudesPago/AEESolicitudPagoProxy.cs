using Api.Gateway.Models.SolicitudesPago.Commands;
using Api.Gateway.Models.SolicitudesPago.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.ServiciosBasicos.AEElectrica.SolicitudesPago
{
    public interface IAEESolicitudPagoProxy
    {
        Task<List<SolicitudPagoDto>> GetAllSolicitudesPago();
        Task<List<SolicitudPagoDto>> GetSolicitudesPagoByAnio(int anio);
        Task<SolicitudPagoDto> GetSolicitudPagoById(int solicitud);
        Task<int> CreateSolicitud([FromBody] SolicitudPagoCreateCommand solcitud);
        Task<int> UpdateSolicitud([FromBody] SolicitudPagoUpdateCommand solcitud);
    }

    public class AEESolicitudPagoProxy: IAEESolicitudPagoProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public AEESolicitudPagoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<SolicitudPagoDto>> GetAllSolicitudesPago()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/solicitudes");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<SolicitudPagoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<SolicitudPagoDto>> GetSolicitudesPagoByAnio(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/solicitudes/getSolicitudesByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<SolicitudPagoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<SolicitudPagoDto> GetSolicitudPagoById(int solicitud)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}aeelectrica/solicitudes/getSolicitudById/{solicitud}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<SolicitudPagoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> CreateSolicitud([FromBody] SolicitudPagoCreateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}aeelectrica/solicitudes/createSolicitud", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> UpdateSolicitud([FromBody] SolicitudPagoUpdateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}aeelectrica/solicitudes/updateSolicitud", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
