using Api.Gateway.Models.LogsEntregables.Commands;
using Api.Gateway.Models.LogsEntregables.DTOs;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.LogEntregables;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.ServiciosBasicos.AEElectrica.LogEntregables
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("aeelectrica/logEntregables")]
    public class LogEntregableController : ControllerBase
    {
        private readonly IAEELogEntregableProxy _logs;

        public LogEntregableController(IAEELogEntregableProxy logs)
        {
            _logs = logs;
        }

        [HttpGet]
        [Route("getHistorialEBySolicitud/{solicitud}")]
        public async Task<List<LogEntregableSBDto>> GetHistorialEBySolicitud(int solicitud)
        {
            return await _logs.GetHistorialEBySolicitud(solicitud);
        }

        [HttpPost]
        [Route("createHistorialE")]
        public async Task<IActionResult> CreateHistorial([FromBody] LogSBEntregableCreateCommand historial)
        {
            int status = await _logs.CreateHistorialE(historial);
            return Ok(status);
        }
    }
}
