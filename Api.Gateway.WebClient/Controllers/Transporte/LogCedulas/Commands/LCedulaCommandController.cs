using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.Historiales;
using Api.Gateway.Proxies.Transporte.LogCedulas.Commands;
using Api.Gateway.Proxies.Transporte.LogCedulas.Queries;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Transporte.LogCedulas.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("transporte/logCedulas")]
    public class LCedulaCommandController : ControllerBase
    {
        private readonly ICLCedulaTransporteProxy _logs;

        public LCedulaCommandController(ICLCedulaTransporteProxy logs)
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
