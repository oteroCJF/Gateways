using Api.Gateway.Models;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Comedor;
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

namespace Api.Gateway.WebClient.Proxy.Comedor.CedulasEvaluacion.Queries
{
    public interface IQCedulaComedorProxy
    {
        Task<List<CedulaComedorDto>> GetAllCedulasEvaluacionAsync();
        Task<CedulaEvaluacionDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaByAnioAsync(int servicio, int anio, string usuario);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaByAnioMes(int servicio, int anio, int mes, int contrato, string usuario);
        Task<CedulaComedorDto> GetCedulaById(int cedula);
        Task<decimal> GetTotalPDAsync(int cedula);
    }

    public class QCedulaComedorProxy : IQCedulaComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QCedulaComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<CedulaComedorDto>> GetAllCedulasEvaluacionAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}Comedor");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CedulaComedorDto>>(
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
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/cedulaEvaluacion/getCedulasByAnio/{servicio}/{anio}/{usuario}");
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
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/cedulaEvaluacion/getCedulasByAnioMes/{servicio}/{anio}/{mes}/{contrato}/{usuario}");
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

        public async Task<CedulaEvaluacionDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/cedulaEvaluacion/getCedulasByInmuebleAM/{inmueble}/{anio}/{mes}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaEvaluacionDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaComedorDto> GetCedulaById(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/cedulaEvaluacion/getCedulaById/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaComedorDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/cedulaEvaluacion/getTotalPD/{cedula}");
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
