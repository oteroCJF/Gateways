using Api.Gateway.Models.Inmuebles.Commands.Direcciones;
using Api.Gateway.Models.Inmuebles.DTOs.Direcciones;
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

namespace Api.Gateway.WebClient.Proxy.Inmuebles.Direcciones
{
    public interface IDireccionProxy 
    {
        Task<List<DireccionDto>> GetAllDirecciones();
        Task<DireccionDto> GetDireccionById(int id);
        Task<DireccionDto> CreateDireccion([FromBody] CreateDireccionCommand request);
        Task<DireccionDto> UpdateDireccion(UpdateDireccionCommand request);
    }

    public class DireccionProxy : IDireccionProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public DireccionProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<DireccionDto>> GetAllDirecciones()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}inmuebles/getDirecciones");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<DireccionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<DireccionDto> GetDireccionById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}inmuebles/getDireccionById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DireccionDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<DireccionDto> CreateDireccion([FromBody] CreateDireccionCommand direccion)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(direccion),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}inmuebles/createDireccion", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DireccionDto>(
                   await request.Content.ReadAsStringAsync(),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
               );
        }

        public async Task<DireccionDto> UpdateDireccion([FromBody] UpdateDireccionCommand direccion)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(direccion),
                   Encoding.UTF8,
            "application/json"
            );
            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}inmuebles/updateDireccion", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DireccionDto>(
                   await request.Content.ReadAsStringAsync(),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
               );
        }
    }
}
