using Api.Gateway.Models.LogEntregables.Commands;
using Api.Gateway.Models.LogEntregables.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.Historiales;
using Api.Gateway.Proxies.Limpieza.Variables;
using Api.Gateway.Proxies.Agua.LogEntregables.Commands;
using Api.Gateway.Proxies.Agua.LogEntregables.Queries;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.LogEntregables.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/logEntregables")]
    public class LEntregableCommandController : ControllerBase
    {
        private readonly ICLEntregableAguaProxy _logs;

        public LEntregableCommandController(ICLEntregableAguaProxy logs)
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
