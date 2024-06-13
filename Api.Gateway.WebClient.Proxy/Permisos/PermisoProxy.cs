using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Modulos.DTOs;
using Api.Gateway.Models.Permisos.Commands;
using Api.Gateway.Models.Permisos.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Permisos
{
    public interface IPermisoProxy
    {
        Task<List<PermisoUsuarioDto>> GetPermisosByUsuario(string usuario);
        Task<List<PermisoUsuarioDto>> GetPermisosByModuloUsuario(string usuario,int modulo);
        Task<List<ModuloDto>> GetModulosByUsuario(string usuario);
        Task CreatePermisos(List<PermisoCreateCommand> permisos);
        Task DeletePermisos(string usuario);
    }

    public class PermisoProxy : IPermisoProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public PermisoProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<PermisoUsuarioDto>> GetPermisosByUsuario(string usuario)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}permisos/getPermisosByUsuario/{usuario}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<PermisoUsuarioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<PermisoUsuarioDto>> GetPermisosByModuloUsuario(string usuario, int modulo)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}permisos/getPermisosByModuloUsuario/{usuario}/{modulo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<PermisoUsuarioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<ModuloDto>> GetModulosByUsuario(string usuario)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}permisos/getModulosByUsuario/{usuario}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<ModuloDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task CreatePermisos(List<PermisoCreateCommand> permisos)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(permisos),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}permisos/createPermisosByUsuario", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task DeletePermisos(string usuario)
        {
            var request = await _httpClient.DeleteAsync($"{_apiGatewayUrl}permisos/deletePermisosByUsuario/{usuario}");
            request.EnsureSuccessStatusCode();
        }
    }
}
