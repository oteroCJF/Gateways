using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Proxies.Transporte.Repositorios.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Transporte.Repositorios.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("transporte/repositorios")]
    public class RepositorioController : ControllerBase
    {

        private readonly ICRepositorioTransporteProxy _repositorios;

        public RepositorioController(ICRepositorioTransporteProxy repositorios)
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
