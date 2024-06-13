using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Fumigacion;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Fumigacion.CedulasEvaluacion
{
    public interface IFCedulaProxy
    {
        Task<List<CedulaFumigacionDto>> GetAllCedulasEvaluacionAsync();
        Task<CedulaFumigacionDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes);
        Task<List<CedulaFumigacionDto>> GetCedulaByAnioAsync(int servicio, int anio, string usuario);
        Task<List<CedulaEvaluacionDto>> GetCedulaByAnioMesAsync(int servicio, int anio, int mes, int contrato, string usuario);
        Task<CedulaFumigacionDto> GetCedulaById(int cedula);
        Task<CedulaFumigacionDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula);
        Task<decimal> GetTotalPDAsync(int cedula);
    }

    public class FCedulaProxy : IFCedulaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public FCedulaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CedulaFumigacionDto>> GetAllCedulasEvaluacionAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaFumigacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CedulaFumigacionDto>> GetCedulaByAnioAsync(int servicio, int anio, string usuario)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/cedulaEvaluacion/getCedulasByAnio/{servicio}/{anio}/{usuario}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaFumigacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CedulaEvaluacionDto>> GetCedulaByAnioMesAsync(int servicio, int anio, int mes, int contrato, string usuario)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/cedulaEvaluacion/getCedulasByAnioMes/{servicio}/{anio}/{mes}/{usuario}/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaEvaluacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaFumigacionDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/cedulaEvaluacion/getCedulasByInmuebleAM/{inmueble}/{anio}/{mes}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaFumigacionDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaFumigacionDto> GetCedulaById(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/cedulaEvaluacion/getCedulaById/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaFumigacionDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaFumigacionDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                   JsonSerializer.Serialize(cedula),
                   Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PutAsync($"{_apiGatewayUrl}fumigacion/cedulaEvaluacion/updateCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaFumigacionDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}fumigacion/cedulaEvaluacion/getTotalPD/{cedula}");
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
