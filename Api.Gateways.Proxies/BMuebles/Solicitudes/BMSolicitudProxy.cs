using Api.Gateway.Models.BMuebles.Solicitudes.Commands;
using Api.Gateway.Models.BMuebles.Solicitudes.DTOs;
using Api.Gateway.Models.Contratos.Commands;
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

namespace Api.Gateway.Proxies.BMuebles.Solicitudes
{
    public interface IBMSolicitudProxy
    {
        Task<List<SolicitudDto>> GetAllSolicitudes();
        Task<SolicitudDto> GetSolicitudById(int id);
        Task<SolicitudDto> CreateSolicitud(SolicitudCreateCommand solicitud);
    }
    
    public class BMSolicitudProxy : IBMSolicitudProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public BMSolicitudProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<SolicitudDto>> GetAllSolicitudes()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.BMueblesUrl}api/bmuebles/solicitudes");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.BMueblesUrl}api/bmuebles/solicitudes/getSolicitudById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<SolicitudDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<SolicitudDto> CreateSolicitud([FromBody] SolicitudCreateCommand solicitud)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(solicitud),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.BMueblesUrl}api/bmuebles/solicitudes/createSolicitud", content);
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
