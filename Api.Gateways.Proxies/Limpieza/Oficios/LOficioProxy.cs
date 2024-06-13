using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusCedulas;
using Api.Gateway.Models.Oficios.Commands;
using Api.Gateway.Models.Oficios.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Limpieza.Oficios
{
    public interface ILOficioProxy
    {
        Task<List<OficioDto>> GetAllOficiosAsync();
        Task<List<OficioDto>> GetOficiosByAnio(int anio);
        Task<OficioDto> GetOficioById(int estatus);
        Task<List<DetalleOficioDto>> GetDetalleOficio(int oficio);
        Task<OficioDto> CreateOficio([FromForm] OficioCreateCommand oficio);
        Task<List<DetalleOficioCreateCommand>> CreateDetalleOficio([FromBody] List<DetalleOficioCreateCommand> dtOficio);
        Task<DetalleOficioDto> DeleteDetalleOficio([FromBody] DetalleOficioDeleteCommand dtOficio);
        Task<OficioDto> CorregirOficio([FromBody] CorregirOficioCommand oficio);
        Task<OficioDto> PagarOficio([FromBody] PagarOficioCommand oficio);
        Task<OficioDto> CancelarOficio([FromBody] CancelarOficioCommand oficio);
        Task<OficioDto> EDGPPTOficio([FromBody] EDGPPTOficioCommand oficio);
    }

    public class LOficioProxy : ILOficioProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public LOficioProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<OficioDto>> GetAllOficiosAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<OficioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<OficioDto>> GetOficiosByAnio(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/getOficiosByAnio/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<OficioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<OficioDto> GetOficioById(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/getOficioById/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<DetalleOficioDto>> GetDetalleOficio(int oficio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/getDetalleOficio/{oficio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<DetalleOficioDto>>(
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/createOficio", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OficioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<DetalleOficioCreateCommand>> CreateDetalleOficio([FromBody] List<DetalleOficioCreateCommand> dtOficio)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(dtOficio),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/createDetalleOficio", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<DetalleOficioCreateCommand>>(
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/deleteDetalleOficio", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/corregirOficio", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/enviarDGPPTOficio", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/pagarOficio", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.LimpiezaUrl}api/limpieza/oficios/cancelarOficio", content);
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
