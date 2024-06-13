using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Proxies.Agua.LogCedulas.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.LogCedulas.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/logCedulas")]
    public class LCedulaCommandController : ControllerBase
    {
        private readonly ICLCedulaAguaProxy _logs;

        public LCedulaCommandController(ICLCedulaAguaProxy logs)
        {
            _logs = logs;
        }

        [HttpPost]
        [Route("createHistorial")]
        public async Task<IActionResult> CreateHistorial([FromBody] LogCedulaCreateCommand historial)
        {
            await _logs.CreateHistorial(historial);
            return Ok();
        }
    }
}
