using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Limpieza;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Limpieza.CedulaEvaluacion
{
    public interface ILCedulaProxy
    {
        Task<List<CedulaLimpiezaDto>> GetAllCedulasEvaluacionAsync();
        Task<CedulaLimpiezaDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes);
        Task<List<CedulaLimpiezaDto>> GetCedulaByAnioAsync(int servicio, int anio, string usuario);
        Task<List<CedulaEvaluacionDto>> GetCedulaByAnioMesAsync(int servicio, int anio, int mes, int contrato, string usuario);
        Task<CedulaLimpiezaDto> GetCedulaById(int cedula);
        Task<CedulaLimpiezaDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula);
        Task<CedulaLimpiezaDto> RechazarCedula([FromBody] CedulaEvaluacionUpdateCommand cedula);
        Task<CedulaLimpiezaDto> CedulaSolicitudRechazo([FromBody] CedulaSRUpdateCommand cedula);
        Task<CedulaLimpiezaDto> DenegarSolicitudRechazo([FromBody] CedulaSRUpdateCommand cedula);
        Task<decimal> GetTotalPDAsync(int cedula);
    }

    public class LCedulaProxy : ILCedulaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public LCedulaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CedulaLimpiezaDto>> GetAllCedulasEvaluacionAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaLimpiezaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CedulaLimpiezaDto>> GetCedulaByAnioAsync(int servicio, int anio, string usuario)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/cedulaEvaluacion/getCedulasByAnio/{servicio}/{anio}/{usuario}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaLimpiezaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CedulaEvaluacionDto>> GetCedulaByAnioMesAsync(int servicio, int anio, int mes, int contrato, string usuario)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/cedulaEvaluacion/getCedulasByAnioMes/{servicio}/{anio}/{mes}/{usuario}/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaEvaluacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaLimpiezaDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/cedulaEvaluacion/getCedulasByInmuebleAM/{inmueble}/{anio}/{mes}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaLimpiezaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaLimpiezaDto> GetCedulaById(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/cedulaEvaluacion/getCedulaById/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaLimpiezaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaLimpiezaDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(cedula),
                   Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}limpieza/cedulaEvaluacion/updateCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaLimpiezaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaLimpiezaDto> RechazarCedula([FromBody] CedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(cedula),
                   Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}limpieza/cedulaEvaluacion/rechazarCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaLimpiezaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaLimpiezaDto> CedulaSolicitudRechazo([FromBody] CedulaSRUpdateCommand cedula)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(cedula),
                   Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}limpieza/cedulaEvaluacion/cedulaSolicitudRechazo", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaLimpiezaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaLimpiezaDto> DenegarSolicitudRechazo([FromBody] CedulaSRUpdateCommand cedula)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(cedula),
                   Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}limpieza/cedulaEvaluacion/denegarSolicitudRechazo", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaLimpiezaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}limpieza/cedulaEvaluacion/getTotalPD/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<decimal>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

    }
}
