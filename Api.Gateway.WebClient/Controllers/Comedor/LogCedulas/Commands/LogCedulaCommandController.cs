using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Proxies.Comedor.LogCedula.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.LogCedulas.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/logCedulas")]
    public class LogCedulaCommandController : ControllerBase
    {
        private readonly ICLCedulaComedorProxy _logs;

        public LogCedulaCommandController(ICLCedulaComedorProxy logs)
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
