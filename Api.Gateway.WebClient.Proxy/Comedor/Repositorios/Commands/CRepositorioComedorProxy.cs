using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.WebClient.Proxy.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Proxy.Comedor.Repositorios.Queries
{
    public interface ICRepositorioComedorProxy
    {
        Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand command);
    }
    public class CRepositorioComedorProxy : ICRepositorioComedorProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public CRepositorioComedorProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        public async Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand facturacion)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(facturacion),
            Encoding.UTF8,
            "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}comedor/repositorios/createRepositorio", content);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
               await request.Content.ReadAsStringAsync(),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
           );
        }

    }
}