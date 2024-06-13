using Api.Gateway.Models.Inmuebles.Commands.Direcciones;
using Api.Gateway.Models.Inmuebles.Commands.SedesUsuarios;
using Api.Gateway.Models.Inmuebles.DTOs.Direcciones;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Inmuebles.Direcciones
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
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public DireccionProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<DireccionDto>> GetAllDirecciones()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getDirecciones");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getDireccionById/{id}");
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

            var request = await _httpClient.PostAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/createDireccion", content);
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

            var request = await _httpClient.PutAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/updateDireccion", content);
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
