﻿using Api.Gateway.Models.Firmantes.Commands;
using Api.Gateway.Models.Firmantes.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Mensajeria.Firmantes.Commands
{
    public interface ICFirmanteMensajeriaProxy
    {
        Task<FirmanteDto> CreateFirmantes([FromBody] FirmanteCreateCommand firmantes);
        Task<FirmanteDto> UpdateFirmantes([FromBody] FirmanteUpdateCommand firmantes);
    }

    public class CFirmanteMensajeriaProxy : ICFirmanteMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CFirmanteMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<FirmanteDto> CreateFirmantes([FromBody] FirmanteCreateCommand firmantes)
        {
            var content = new StringContent(
            JsonSerializer.Serialize(firmantes),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}mensajeria/firmantes/createFirmantes", content);
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

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}mensajeria/firmantes/updateFirmantes", content);
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
