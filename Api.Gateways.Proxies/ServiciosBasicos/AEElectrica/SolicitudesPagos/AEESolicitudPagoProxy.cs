using Api.Gateway.Models.SolicitudesPago.Commands;
using Api.Gateway.Models.SolicitudesPago.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.SolicitudesPagos
{

    public interface IAEESolicitudPagoProxy
    {
        Task<List<SolicitudPagoDto>> GetAllSolicitudesPago();
        Task<List<SolicitudPagoDto>> GetSolicitudesPagoByAnio(int anio);
        Task<SolicitudPagoDto> GetSolicitudPagoById(int solicitud);
        Task<int> CreateSolicitud([FromBody] SolicitudPagoCreateCommand solcitud);
        Task<int> UpdateSolicitud([FromBody] SolicitudPagoUpdateCommand solcitud);
    }
    public class AEESolicitudPagoProxy : IAEESolicitudPagoProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public AEESolicitudPagoProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<SolicitudPagoDto>> GetAllSolicitudesPago()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/solicitudes");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/solicitudes/getSolicitudesByAnio/{anio}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/solicitudes/getSolicitudById/{solicitud}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<SolicitudPagoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<int> CreateSolicitud([FromForm] SolicitudPagoCreateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/solicitudes/createSolicitud", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<int> UpdateSolicitud([FromForm] SolicitudPagoUpdateCommand contrato)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(contrato),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PutAsync($"{_apiUrls.AEElectricaUrl}api/aeelectrica/solicitudes/updateSolicitud", content);
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
