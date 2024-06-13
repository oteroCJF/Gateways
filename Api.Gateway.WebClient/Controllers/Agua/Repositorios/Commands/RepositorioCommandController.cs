using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Proxies.Agua.Repositorios.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.Repositorios.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/repositorios")]
    public class RepositorioController : ControllerBase
    {

        private readonly ICRepositorioAguaProxy _repositorios;

        public RepositorioController(ICRepositorioAguaProxy repositorios)
        {
            _repositorios = repositorios;
        }

        [Route("createRepositorio")]
        [HttpPost]
        public async Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand repositorio)
        {
            return await _repositorios.CreateRepositorio(repositorio);
        }
    }
}
