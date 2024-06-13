using Api.Gateway.Models.LogEntregables.Commands;
using Api.Gateway.Models.LogEntregables.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.Historiales;
using Api.Gateway.Proxies.Limpieza.Variables;
using Api.Gateway.Proxies.Mensajeria.LogEntregables.Commands;
using Api.Gateway.Proxies.Mensajeria.LogEntregables.Queries;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.LogEntregables.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/logEntregables")]
    public class LEntregableCommandController : ControllerBase
    {
        private readonly ICLEntregableMensajeriaProxy _logs;

        public LEntregableCommandController(ICLEntregableMensajeriaProxy logs)
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
