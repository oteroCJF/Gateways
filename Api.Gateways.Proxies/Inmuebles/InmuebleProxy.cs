using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.Models.Inmuebles.Commands;
using Api.Gateway.Models.Inmuebles.Commands.SedesUsuarios;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.Inmuebles.DTOs.InmueblesServicio;
using Api.Gateway.Models.Inmuebles.DTOs.InmuebleSU;
using Api.Gateway.Models.Inmuebles.DTOs.InmueblesUS;
using Api.Gateway.Models.Permisos.Commands;
using Api.Gateway.Models.Usuarios.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies
{
    public interface IInmuebleProxy
    {
        Task<List<InmuebleDto>> GetAllInmueblesAsync();
        Task<List<InmuebleDto>> GetAllAdministraciones();
        Task<List<InmuebleServicioDto>> GetAllInmueblesServicio();
        Task<List<InmuebleDto>> GetInmueblesByAdministracion(int administracion);
        Task<List<InmuebleServicioDto>> GetInmueblesByServicio(int servicio);
        Task<List<InmuebleUSDto>> GetAllInmueblesUsuarios(int servicio);
        Task<List<InmuebleUSDto>> GetInmueblesByUsuario(string usuario);
        Task<InmuebleDto> GetSedeByUsuario(string usuario);
        Task<List<InmuebleUSDto>> GetInmueblesByUsuarioServicio(string usuario, int servicio);
        Task<List<InmuebleDto>> GetAdministracionesByUsuarioServicio(string usuario, int servicio);
        Task<InmuebleDto> GetInmuebleById(int inmueble);
        Task CreateInmuebleUS([FromBody] List<CreateCommandInmuebleUS> inmuebleUS);
        Task DeleteInmuebleUS(string usuario);
        Task<InmuebleDto> CreateSedeByUsuario([FromBody] CreateSedeUsuarioCommand sede);
        Task<int> DeleteSedeByUsuario(string usuario);
    }
    public class InmuebleProxy : IInmuebleProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public InmuebleProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<InmuebleDto>> GetAllInmueblesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getInmuebles");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<InmuebleDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<InmuebleDto>> GetAllAdministraciones()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getAdministraciones");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<InmuebleDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<InmuebleServicioDto>> GetAllInmueblesServicio()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getInmueblesServicio");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<InmuebleServicioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<InmuebleDto>> GetInmueblesByAdministracion(int administracion)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getInmueblesByAdministracion/{administracion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<InmuebleDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<InmuebleUSDto>> GetAllInmueblesUsuarios(int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getInmueblesUsuarios/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<InmuebleUSDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<InmuebleServicioDto>> GetInmueblesByServicio(int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getInmueblesByServicio/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<InmuebleServicioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<InmuebleUSDto>> GetInmueblesByUsuario(string usuario)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getInmueblesByUsuario/{usuario}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<InmuebleUSDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<InmuebleUSDto>> GetInmueblesByUsuarioServicio(string usuario, int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getInmueblesByUsuarioServicio/{usuario}/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<InmuebleUSDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<InmuebleDto>> GetAdministracionesByUsuarioServicio(string usuario, int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getAdministracionesByUsuarioServicio/{usuario}/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<InmuebleDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<InmuebleDto> GetInmuebleById(int inmueble)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getInmuebleById/{inmueble}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<InmuebleDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new InmuebleDto();
            }
        }
        
        public async Task<InmuebleDto> GetSedeByUsuario(string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/getSedeByUsuario/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<InmuebleDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new InmuebleDto();
            }
        }

        public async Task CreateInmuebleUS([FromBody] List<CreateCommandInmuebleUS> inmuebleUS)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(inmuebleUS),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/createInmuebleUS", content);
            request.EnsureSuccessStatusCode();
        }

        public async Task DeleteInmuebleUS(string usuario)
        {
            var request = await _httpClient.DeleteAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/deleteInmuebleUS/{usuario}");
            request.EnsureSuccessStatusCode();
        }

        public async Task<InmuebleDto> CreateSedeByUsuario([FromBody] CreateSedeUsuarioCommand sede)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(sede),
                   Encoding.UTF8,
                   "application/json"
               );

            var request = await _httpClient.PostAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/createSede", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<InmuebleDto>(
                   await request.Content.ReadAsStringAsync(),
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
               );
        }

        public async Task<int> DeleteSedeByUsuario(string usuario)
        {
            var request = await _httpClient.DeleteAsync($"{_apiUrls.InmueblesUrl}api/inmuebles/deleteSede/{usuario}");
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
