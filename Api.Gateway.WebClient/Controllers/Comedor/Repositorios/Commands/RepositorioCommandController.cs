using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Proxies.Comedor.Repositorios.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.Repositorios.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/repositorios")]
    public class RepositorioController : ControllerBase
    {

        private readonly ICRepositorioComedorProxy _repositorios;

        public RepositorioController(ICRepositorioComedorProxy repositorios)
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
