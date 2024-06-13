using Api.Gateway.Models;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Agua;
using Api.Gateway.Models.Dashboard.Cedulas;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Agua.CedulasEvaluacion.Queries
{
    public interface IQCedulaAguaProxy
    {
        Task<List<CedulaAguaDto>> GetAllCedulasEvaluacionAsync();
        Task<CedulaAguaDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaByAnioAsync(int servicio, int anio, string usuario);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaByAnioMes(int servicio, int anio, int mes, int contrato, string usuario);
        Task<CedulaAguaDto> GetCedulaById(int cedula);
        Task<decimal> GetTotalPDAsync(int cedula);
    }

    public class QCedulaAguaProxy : IQCedulaAguaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QCedulaAguaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CedulaAguaDto>> GetAllCedulasEvaluacionAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}Agua");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaAguaDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaByAnioAsync(int servicio, int anio, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/cedulaEvaluacion/getCedulasByAnio/{servicio}/{anio}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DataCollection<CedulaEvaluacionDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new DataCollection<CedulaEvaluacionDto>();
            }
            
        }

        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaByAnioMes(int servicio, int anio, int mes, int contrato, string usuario)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/cedulaEvaluacion/getCedulasByAnioMes/{servicio}/{anio}/{mes}/{contrato}/{usuario}");
                request.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<DataCollection<CedulaEvaluacionDto>>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (HttpRequestException e)
            {
                return new DataCollection<CedulaEvaluacionDto>();
            }           
        }

        public async Task<CedulaAguaDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/cedulaEvaluacion/getCedulasByInmuebleAM/{inmueble}/{anio}/{mes}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaAguaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaAguaDto> GetCedulaById(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/cedulaEvaluacion/getCedulaById/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaAguaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}agua/cedulaEvaluacion/getTotalPD/{cedula}");
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
