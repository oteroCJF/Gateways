using System.Net.Http;
using System.Net.Http.Headers;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;

namespace Api.Gateway.Proxies.Transporte.EntregablesContrato.Queries
{
    public interface IQEContratoTransporteProxy
    {
        Task<List<EContratoDto>> GetEntregableContratacionByContrato(int contrato);
        Task<List<EContratoDto>> GetEntregableContratacionByContratoConvenio(int contrato, int convenio);
        Task<EContratoDto> GetEntregableById(int entregable);
        Task<string> VisualizarEntregablesCont(string contrato, string tipoEntregable, string archivo);
        Task<string> VisualizarEntregablesConv(string contrato, string convenio, string tipoEntregable, string archivo);
    }
    public class QEContratoTransporteProxy : IQEContratoTransporteProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public QEContratoTransporteProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }
        public async Task<List<EContratoDto>> GetEntregableContratacionByContrato(int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/entregablesContratacion/getEntregablesByContrato/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<List<EContratoDto>> GetEntregableContratacionByContratoConvenio(int contrato, int convenio)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/entregablesContratacion/getEntregablesByContratoConvenio/{contrato}/{convenio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<EContratoDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<EContratoDto> GetEntregableById(int entregable)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/entregablesContratacion/getEntregableById/{entregable}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EContratoDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        public async Task<string> VisualizarEntregablesCont(string contrato, string tipoEntregable, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/entregablesContratacion/visualizarEntregableCont/{contrato}/{tipoEntregable}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;
        }
        public async Task<string> VisualizarEntregablesConv(string contrato, string convenio, string tipoEntregable, string archivo)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.TransporteUrl}api/transporte/entregablesContratacion/visualizarEntregableConv/{contrato}/{convenio}/{tipoEntregable}/{archivo}");
            request.EnsureSuccessStatusCode();

            var contents = await request.Content.ReadAsStringAsync();

            return contents;

        }
    }
}
