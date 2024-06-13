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

namespace Api.Gateway.Proxies.Agua.Oficios.Commands
{
    public interface ICOficioAguaProxy
    {
        Task<OficioDto> CreateOficio([FromForm] OficioCreateCommand oficio);
        Task<List<DetalleOficioCreateCommand>> CreateDetalleOficio([FromBody] List<DetalleOficioCreateCommand> dtOficio);
        Task<DetalleOficioDto> DeleteDetalleOficio([FromBody] DetalleOficioDeleteCommand dtOficio);
        Task<OficioDto> CorregirOficio([FromBody] CorregirOficioCommand oficio);
        Task<OficioDto> PagarOficio([FromBody] PagarOficioCommand oficio);
        Task<OficioDto> CancelarOficio([FromBody] CancelarOficioCommand oficio);
        Task<OficioDto> EDGPPTOficio([FromBody] EDGPPTOficioCommand oficio);
    }

    public class COficioAguaProxy : ICOficioAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public COficioAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
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

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/oficios/createOficio", formContent);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/oficios/createDetalleOficio", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/oficios/deleteDetalleOficio", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/oficios/corregirOficio", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/oficios/enviarDGPPTOficio", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/oficios/pagarOficio", content);
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

            var request = await _httpClient.PostAsync($"{_apiUrls.AguaUrl}api/agua/oficios/cancelarOficio", content);
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
