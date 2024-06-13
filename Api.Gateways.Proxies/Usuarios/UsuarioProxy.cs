using Api.Gateway.Models;
using Api.Gateway.Models.Usuarios.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Usuarios
{
    public interface IUsuarioProxy
    {
        Task<List<UsuarioDto>> GetAllUsuariosAsync();
        Task<UsuarioDto> GetUsuarioByIdAsync(string id);
    }

    public class UsuarioProxy : IUsuarioProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public UsuarioProxy(HttpClient httpClient,IOptions<ApiUrls> apiUrls,IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<UsuarioDto>> GetAllUsuariosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.IdentityUrl}api/usuarios");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<UsuarioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<UsuarioDto> GetUsuarioByIdAsync(string id)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiUrls.IdentityUrl}api/usuarios/{id}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<UsuarioDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            { 
                return new UsuarioDto();
            }
        }
    }
}
