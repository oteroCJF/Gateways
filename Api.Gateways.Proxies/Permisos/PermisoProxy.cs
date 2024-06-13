using Api.Gateway.Models.Modulos.DTOs;
using Api.Gateway.Models.Permisos.Commands;
using Api.Gateway.Models.Permisos.DTOs;
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

namespace Api.Gateway.Proxies.Permisos
{
    public interface IPermisoProxy
    {
        Task<PermisoDto> GetPermisosByIdAsync(int permiso);
        Task<IEnumerable<PermisoUsuarioDto>> GetPermisosUsuarioAsync(string usuario);
        Task<List<PermisoUsuarioDto>> GetPermisosByModuloUsuarioAsync(string usuario, int modulo);
        Task<List<PermisoSubmoduloDto>> GetPermisosBySubmoduloAsync(int submodulo);
        Task CreatePermisos(List<PermisoCreateCommand> permisos);
        Task DeletePermisos(PermisoDeleteCommand permisos);
    }

    public class PermisoProxy: IPermisoProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public PermisoProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<PermisoDto> GetPermisosByIdAsync(int permiso)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.PermisosUrl}api/permisos/{permiso}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<PermisoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<IEnumerable<PermisoUsuarioDto>> GetPermisosUsuarioAsync(string usuario)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.PermisosUrl}api/permisos/getPermisosByUsuario/{usuario}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<IEnumerable<PermisoUsuarioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<List<PermisoUsuarioDto>> GetPermisosByModuloUsuarioAsync(string usuario, int modulo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.PermisosUrl}api/permisos/getPermisosByModuloUsuario/{usuario}/{modulo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<PermisoUsuarioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }        
        public async Task<List<PermisoSubmoduloDto>> GetPermisosBySubmoduloAsync(int submodulo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.PermisosUrl}api/permisos/getPermisosBySubmodulo/{submodulo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<PermisoSubmoduloDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task CreatePermisos([FromBody] List<PermisoCreateCommand> permisos)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(permisos),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.PermisosUrl}api/permisos/createPermisosByUsuario", content);
            request.EnsureSuccessStatusCode();
        }
        public async Task DeletePermisos([FromBody] PermisoDeleteCommand permisos)
        {
            var request = await _httpClient.DeleteAsync($"{_apiUrls.PermisosUrl}api/permisos/deletePermisosByUsuario/{permisos.UsuarioId}");
            request.EnsureSuccessStatusCode();
        }
    }
}
