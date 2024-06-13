using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Proxies.Mensajeria.Repositorios.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Repositorios.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/repositorios")]
    public class RepositorioController : ControllerBase
    {

        private readonly ICRepositorioMensajeriaProxy _repositorios;

        public RepositorioController(ICRepositorioMensajeriaProxy repositorios)
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
