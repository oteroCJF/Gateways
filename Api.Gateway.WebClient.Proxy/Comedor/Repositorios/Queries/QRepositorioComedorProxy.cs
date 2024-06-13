using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Models.Repositorios.DTOs;
using Api.Gateway.Proxies.Comedor.Repositorios.Queries;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateway.WebClient.Proxy.Comedor.Oficios.Queries;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.Repositorios.Commands
{
    public interface IQRepositorioComedorProxy
    {
        Task<List<RepositorioDto>> GetAllRepositorios(int anio);
        Task<RepositorioDto> GetRepositorioById(int facturacion);
        Task<RepositorioDto> GetRepositorioByAMC(int anio, int mes, int contrato);
    }
    public class QRepositorioComedorProxy : IQRepositorioComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public QRepositorioComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<List<RepositorioDto>> GetAllRepositorios(int anio)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/repositorios/{anio}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<RepositorioDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<RepositorioDto> GetRepositorioByAMC(int anio, int mes, int contrato)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/repositorios/getRepositorioByAMC/{anio}/{mes}/{contrato}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<RepositorioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<RepositorioDto> GetRepositorioById(int facturacion)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}comedor/repositorios/getRepositorioById/{facturacion}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<RepositorioDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
