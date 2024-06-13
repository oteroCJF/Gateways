using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Fumigacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Limpieza;

namespace Api.Gateway.Proxies.Fumigacion.CedulasEvaluacion
{
    public interface IFCedulaProxy
    {
        Task<List<CedulaFumigacionDto>> GetAllCedulasAsync();
        Task<List<CedulaFumigacionDto>> GetCedulaEvaluacionByAnio(int anio);
        Task<List<CedulaFumigacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes);
        Task<CedulaFumigacionDto> GetCedulaEvaluacionByInmuebleAnioMes(int inmueble, int anio, int mes);
        Task<CedulaFumigacionDto> GetCedulaById(int cedula);
        Task<decimal> GetTotalPDAsync(int cedula);
        Task<CedulaFumigacionDto> EnviarCedula([FromBody] EnviarCedulaEvaluacionUpdateCommand cedula);
        Task<CedulaFumigacionDto> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand cedula);
    }

    public class FCedulaProxy : IFCedulaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public FCedulaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CedulaFumigacionDto>> GetAllCedulasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/cedulaEvaluacion");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaFumigacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CedulaFumigacionDto>> GetCedulaEvaluacionByAnio(int anio)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/cedulaEvaluacion/getCedulasByAnio/{anio}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<List<CedulaFumigacionDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch(HttpRequestException ex)
            {
                return new List<CedulaFumigacionDto>();
            }
        }

        public async Task<List<CedulaFumigacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/cedulaEvaluacion/getCedulasByAnioMes/{anio}/{mes}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaFumigacionDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaFumigacionDto> GetCedulaEvaluacionByInmuebleAnioMes(int inmueble, int anio, int mes)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/cedulaEvaluacion/getCedulasByInmuebleAM/{inmueble}/{anio}/{mes}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/cedulaEvaluacion/getCedulaById/{cedula}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/cedulaEvaluacion/getTotalPD/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<decimal>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaFumigacionDto> EnviarCedula([FromBody] EnviarCedulaEvaluacionUpdateCommand cedula)
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(cedula),
                  Encoding.UTF8,
                  "application/json"
              );

            var request = await _httpClient.PutAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/cedulaEvaluacion/enviarCedula", content);
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

            var request = await _httpClient.PutAsync($"{_apiUrls.FumigacionUrl}api/fumigacion/cedulaEvaluacion/updateCedula", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaFumigacionDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
