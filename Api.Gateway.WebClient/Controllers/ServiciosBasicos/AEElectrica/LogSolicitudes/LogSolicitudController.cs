using Api.Gateway.Models.LogSolicitudes.Commands;
using Api.Gateway.Models.LogSolicitudes.DTOs;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.LogSolicitudes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.ServiciosBasicos.AEElectrica.LogSolicitudes
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("aeelectrica/logSolicitudes")]
    public class LogSolicitudController : ControllerBase
    {
        private readonly IAEELogSolicitudProxy _logs;

        public LogSolicitudController(IAEELogSolicitudProxy logs)
        {
            _logs = logs;
        }

        [HttpGet]
        [Route("getHistorialBySolicitud/{solicitud}")]
        public async Task<List<LogSolicitudDto>> GetHistorialBySolicitud(int solicitud)
        {
            return await _logs.GetHistorialBySolicitud(solicitud);
        }

        [HttpPost]
        [Route("createHistorial")]
        public async Task<IActionResult> CreateHistorial([FromBody] LogSolicitudCreateCommand historial)
        {
            int status = await _logs.CreateHistorial(historial);
            return Ok(status);
        }
    }
}
