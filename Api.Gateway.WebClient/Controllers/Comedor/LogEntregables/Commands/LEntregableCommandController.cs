using Api.Gateway.Models.LogEntregables.Commands;
using Api.Gateway.Models.LogEntregables.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Comedor.LogEntregables.Commands;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.LogEntregables.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/logEntregables")]
    public class LEntregableCommandController : ControllerBase
    {
        private readonly ICLEntregableComedorProxy _logs;

        public LEntregableCommandController(ICLEntregableComedorProxy logs)
        {
            _logs = logs;
        }

        [HttpPost]
        [Route("createHistorial")]
        public async Task<IActionResult> CreateHistorial([FromBody] LogEntregableCreateCommand historial)
        {
            await _logs.CreateHistorial(historial);
            return Ok();
        }
    }
}
