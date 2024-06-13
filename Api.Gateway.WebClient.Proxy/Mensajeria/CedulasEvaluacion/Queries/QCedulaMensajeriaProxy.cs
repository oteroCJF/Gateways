using Api.Gateway.Models;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Mensajeria;
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

namespace Api.Gateway.WebClient.Proxy.Mensajeria.CedulasEvaluacion.Queries
{
    public interface IQCedulaMensajeriaProxy
    {
        Task<DataCollection<CedulaEvaluacionDto>> GetReportePAT(int anio, int mes);
        Task<CedulaMensajeriaDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaByAnioAsync(int servicio, int anio, string usuario);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaByAnioMes(int servicio, int anio, int mes, int contrato, string usuario);
        Task<CedulaMensajeriaDto> GetCedulaById(int cedula);
        Task<decimal> GetTotalPDAsync(int cedula);
    }

    public class QCedulaMensajeriaProxy : IQCedulaMensajeriaProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QCedulaMensajeriaProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<DataCollection<CedulaEvaluacionDto>> GetReportePAT(int anio, int mes)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cedulaEvaluacion/getReportePAT/{anio}/{mes}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DataCollection<CedulaEvaluacionDto>>(
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
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cedulaEvaluacion/getCedulasByAnio/{servicio}/{anio}/{usuario}");
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
                var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cedulaEvaluacion/getCedulasByAnioMes/{servicio}/{anio}/{mes}/{contrato}/{usuario}");
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

        public async Task<CedulaMensajeriaDto> GetCedulaByInmuebleAnioMesAsync(int inmueble, int anio, int mes)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cedulaEvaluacion/getCedulasByInmuebleAM/{inmueble}/{anio}/{mes}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaMensajeriaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CedulaMensajeriaDto> GetCedulaById(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cedulaEvaluacion/getCedulaById/{cedula}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CedulaMensajeriaDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}mensajeria/cedulaEvaluacion/getTotalPD/{cedula}");
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
