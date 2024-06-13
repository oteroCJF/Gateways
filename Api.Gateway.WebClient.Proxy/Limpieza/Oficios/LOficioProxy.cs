using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Dashboard.Cedulas;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Oficios.Commands;
using Api.Gateway.Models.Oficios.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Limpieza.Oficios
{
    public interface ILOficioProxy
    {
        Task<List<OficioDto>> GetAllOficiosAsync();
        Task<List<OficioDto>> GetOficiosByAnio(int anio);
        Task<OficioDto> GetOficioById(int id);
        Task<List<CFDIDto>> GetFacturasNCPendientes(int oficio);
        Task<OficioDto> CreateOficio([FromForm] OficioCreateCommand oficio);
        Task<List<DetalleOficioDto>> CreateDetalleOficio([FromBody] List<DetalleOficioCreateCommand> oficio);
        Task<DetalleOficioDto> DeleteDetalleOficio([FromBody] DetalleOficioDeleteCommand oficio);
        Task<OficioDto> CorregirOficio([FromBody] CorregirOficioCommand oficio);
        Task<OficioDto> PagarOficio([FromBody] PagarOficioCommand oficio);
        Task<OficioDto> CancelarOficio([FromBody] CancelarOficioCommand oficio);
        Task<OficioDto> EDGPPTOficio([FromBody] EDGPPTOficioCommand oficio);
    }

    public class LOficioProxy : ILOficioProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public LOficioProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<OficioDto>> GetAllOficiosAsync()
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/oficios");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<OficioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            } 
            catch (HttpRequestException ex)
            { 
                return new List<OficioDto>();
            }
        }
        
        public async Task<List<OficioDto>> GetOficiosByAnio(int anio)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/oficios/getOficiosByAnio/{anio}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<OficioDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            } 
            catch (HttpRequestException ex)
            { 
                return new List<OficioDto>();
            }
        }
        
        public async Task<OficioDto> GetOficioById(int id)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/oficios/getOficioById/{id}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<OficioDto>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            } 
            catch (HttpRequestException ex)
            { 
                return new OficioDto();
            }
        }

        public async Task<List<CFDIDto>> GetFacturasNCPendientes(int oficio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/oficios/getFacturasNCPendientes/{oficio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CFDIDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<OficioDto> CreateOficio([FromForm] OficioCreateCommand oficio)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(oficio.Anio.ToString()), "Anio");
            formContent.Add(new StringContent(oficio.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(oficio.NumeroOficio.ToString()), "NumeroOficio");
            formContent.Add(new StringContent(oficio.ContratoId.ToString()), "ContratoId");
            formContent.Add(new StringContent(oficio.ServicioId.ToString()), "ServicioId");
            formContent.Add(new StringContent(oficio.FechaTramitado.ToString()), "FechaTramitado");
            if (oficio.Oficio != null)
            {
                var oficioContent = new StreamContent(oficio.Oficio.OpenReadStream());
                oficioContent.Headers.ContentType = MediaTypeHeaderValue.Parse(oficio.Oficio.ContentType);
                formContent.Add(oficioContent, name: "Oficio", oficio.Oficio.FileName);
            }

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/oficios/createOficio", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<DetalleOficioDto>> CreateDetalleOficio([FromBody] List<DetalleOficioCreateCommand> dtOficio)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(dtOficio),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/oficios/createDetalleOficio", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<DetalleOficioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<DetalleOficioDto> DeleteDetalleOficio([FromBody] DetalleOficioDeleteCommand dtOficio)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(dtOficio),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/oficios/deleteDetalleOficio", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DetalleOficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<OficioDto> CorregirOficio([FromBody] CorregirOficioCommand oficio)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(oficio),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/oficios/corregirOficio", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<OficioDto> PagarOficio([FromBody] PagarOficioCommand oficio)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(oficio),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/oficios/pagarOficio", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<OficioDto> CancelarOficio([FromBody] CancelarOficioCommand oficio)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(oficio),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/oficios/cancelarOficio", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<OficioDto> EDGPPTOficio([FromBody] EDGPPTOficioCommand oficio)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(oficio),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}limpieza/oficios/eDGPPTOficio", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

    }
}
