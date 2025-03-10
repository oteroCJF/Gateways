﻿using Api.Gateway.Models.Firmantes.Commands;
using Api.Gateway.Models.Firmantes.DTOs;
using Api.Gateway.Proxies.Comedor.Firmantes.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.BMuebles.Firmantes.Queries
{
    public interface IQFirmanteBMueblesProxy
    {
        Task<List<FirmanteDto>> GetAllFirmantesAsync();
        Task<FirmanteDto> GetFirmanteById(int firmante);
        Task<List<FirmanteDto>> GetFirmantesByInmueble(int inmueble);
        Task<FirmanteDto> CreateFirmantes([FromBody] FirmanteCreateCommand firmantes);
        Task<FirmanteDto> UpdateFirmantes([FromBody] FirmanteUpdateCommand firmantes);
    }

    public class QFirmanteBMueblesProxy : IQFirmanteBMueblesProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QFirmanteBMueblesProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<FirmanteDto>> GetAllFirmantesAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}bmuebles/firmantes");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FirmanteDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<FirmanteDto> GetFirmanteById(int firmante)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}bmuebles/firmantes/getFirmanteById/{firmante}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<FirmanteDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<FirmanteDto>> GetFirmantesByInmueble(int inmueble)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}bmuebles/firmantes/getFirmantesByInmueble/{inmueble}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<FirmanteDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<FirmanteDto> CreateFirmantes([FromBody] FirmanteCreateCommand firmantes)
        {
            var content = new StringContent(
            JsonSerializer.Serialize(firmantes),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}bmuebles/firmantes/createFirmantes", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<FirmanteDto>(
               await request.Content.ReadAsStringAsync(),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
           );
        }

        public async Task<FirmanteDto> UpdateFirmantes([FromBody] FirmanteUpdateCommand firmantes)
        {
            var content = new StringContent(
            JsonSerializer.Serialize(firmantes),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}bmuebles/firmantes/updateFirmantes", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<FirmanteDto>(
               await request.Content.ReadAsStringAsync(),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
           );
        }
    }
}
