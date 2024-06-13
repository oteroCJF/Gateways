using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusCedulas;
using Api.Gateway.Models.Oficios.Commands;
using Api.Gateway.Models.Oficios.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Proxies.Mensajeria.Oficios.Commands;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Mensajeria.Oficios.Queries
{
    public interface IQOficioMensajeriaProxy
    {
        Task<List<OficioDto>> GetAllOficiosAsync();
        Task<List<OficioDto>> GetOficiosByAnio(int anio);
        Task<OficioDto> GetOficioById(int estatus);
        Task<List<DetalleOficioDto>> GetDetalleOficio(int oficio);
    }

    public class QOficioMensajeriaProxy : IQOficioMensajeriaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QOficioMensajeriaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<OficioDto>> GetAllOficiosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/oficios");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<OficioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<OficioDto>> GetOficiosByAnio(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/oficios/getOficiosByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<OficioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<OficioDto> GetOficioById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/oficios/getOficioById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<DetalleOficioDto>> GetDetalleOficio(int oficio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.MensajeriaUrl}api/mensajeria/oficios/getDetalleOficio/{oficio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<DetalleOficioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
