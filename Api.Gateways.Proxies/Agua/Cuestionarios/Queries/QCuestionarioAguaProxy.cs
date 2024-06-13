﻿using Api.Gateway.Models.Cuestionarios.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.Proxies.Agua.Cuestionarios.Queries
{
    public interface IQCuestionarioAguaProxy
    {
        Task<List<CuestionarioDto>> GetAllPreguntasAsync();
        Task<List<CuestionarioMensualDto>> GetCuestionarioMensualId(int anio, int mes, int contrato);
        Task<List<CuestionarioMensualDto>> GetPreguntasConDeductiva(int anio, int mes, int contrato);
        Task<CuestionarioDto> GetPreguntaById(int pregunta);
    }

    public class QCuestionarioAguaProxy : IQCuestionarioAguaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QCuestionarioAguaProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<List<CuestionarioDto>> GetAllPreguntasAsync()
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/cuestionarios");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CuestionarioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<List<CuestionarioMensualDto>> GetCuestionarioMensualId(int anio, int mes, int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/cuestionarios/{anio}/{mes}/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<CuestionarioMensualDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        
        public async Task<List<CuestionarioMensualDto>> GetPreguntasConDeductiva(int anio, int mes, int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/cuestionarios/getPreguntasConDeductiva/{anio}/{mes}/{contrato}");
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
            var request = await _httpClient.GetAsync($"{_apiUrls.AguaUrl}api/agua/cuestionarios/getPreguntaById/{pregunta}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CuestionarioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
