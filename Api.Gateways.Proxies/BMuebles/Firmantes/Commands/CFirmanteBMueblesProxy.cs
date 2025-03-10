﻿using Api.Gateway.Models.Firmantes.Commands;
using Api.Gateway.Models.Firmantes.DTOs;
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

namespace Api.Gateway.Proxies.BMuebles.Firmantes.Commands
{
    public interface ICFirmanteBMueblesProxy
    {
        Task<FirmanteDto> CreateFirmantes([FromBody] FirmanteCreateCommand firmantes);
        Task<FirmanteDto> UpdateFirmantes([FromBody] FirmanteUpdateCommand firmantes);
    }

    public class CFirmanteBMueblesProxy : ICFirmanteBMueblesProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CFirmanteBMueblesProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<FirmanteDto> CreateFirmantes([FromBody] FirmanteCreateCommand firmantes)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(firmantes),
                Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.BMueblesUrl}api/bmuebles/firmantes/createFirmantes", content);
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

            var request = await _httpClient.PutAsync($"{_apiUrls.BMueblesUrl}api/bmuebles/firmantes/updateFirmantes", content);
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
