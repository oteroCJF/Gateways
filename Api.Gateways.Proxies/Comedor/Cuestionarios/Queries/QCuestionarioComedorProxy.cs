using Api.Gateway.Models.Cuestionarios.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Comedor.Cuestionarios.Queries
{
    public interface IQCuestionarioComedorProxy
    {
        Task<List<CuestionarioDto>> GetAllPreguntasAsync();
        Task<List<CuestionarioMensualDto>> GetCuestionarioMensualId(int anio, int mes, int contrato, int servicio);
        Task<CuestionarioDto> GetPreguntaById(int pregunta);
        Task<List<int>> GetPreguntasDeductiva(int anio, int mes, int contrato, int servicio);
    }

    public class QCuestionarioComedorProxy : IQCuestionarioComedorProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QCuestionarioComedorProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CuestionarioDto>> GetAllPreguntasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cuestionarios");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CuestionarioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CuestionarioMensualDto>> GetCuestionarioMensualId(int anio, int mes, int contrato, int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cuestionarios/{anio}/{mes}/{contrato}/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CuestionarioMensualDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<CuestionarioDto> GetPreguntaById(int pregunta)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cuestionarios/getPreguntaById/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CuestionarioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<int>> GetPreguntasDeductiva(int anio, int mes, int contrato, int servicio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.ComedorUrl}api/comedor/cuestionarios/getPreguntasDeductiva/{anio}/{mes}/{contrato}/{servicio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<int>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}


