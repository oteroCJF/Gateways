using Api.Gateway.Models;
using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Meses.DTOs;
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

namespace Api.Gateway.Proxies.Catalogos.CTParametros
{
    public interface ICTParametroProxy
    {
        Task<List<CTParametroDto>> GetAllParametrosAsync();
        Task<CTParametroDto> GetParametroById(int parametro);
        Task<List<CTParametroDto>> GetParametroByTipo(string tipo);
        Task<List<CTParametroDto>> GetParametroByTabla(string tabla);
    }

    public class CTParametroProxy : ICTParametroProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CTParametroProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CTParametroDto>> GetAllParametrosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/parametros");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTParametroDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CTParametroDto> GetParametroById(int parametro)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/parametros/getParametroById/{parametro}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CTParametroDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTParametroDto>> GetParametroByTipo(string tipo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/parametros/getParametroByTipo/{tipo}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTParametroDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CTParametroDto>> GetParametroByTabla(string tabla)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogosUrl}api/catalogos/parametros/getParametroByTabla/{tabla}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CTParametroDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
