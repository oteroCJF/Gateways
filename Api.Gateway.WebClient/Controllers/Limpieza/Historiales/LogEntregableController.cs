using Api.Gateway.Models.LogEntregables.Commands;
using Api.Gateway.Models.LogEntregables.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.Historiales;
using Api.Gateway.Proxies.Limpieza.Variables;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.Historiales
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/logEntregables")]
    public class LogEntregableController : ControllerBase
    {
        private readonly ILLogEntregableProxy _logs;
        private readonly IEstatusEntregableProxy _estatus;
        private readonly ICTEntregableProxy _centregables;
        private readonly IUsuarioProxy _usuarios;

        public LogEntregableController(ILLogEntregableProxy logs, IEstatusEntregableProxy estatus, IUsuarioProxy usuarios,
                                        ICTEntregableProxy centregables)
        {
            _logs = logs;
            _estatus = estatus;
            _usuarios = usuarios;
            _centregables = centregables;
        }

        [HttpGet]
        [Route("getHistorialEntregablesByCedula/{cedula}")]
        public async Task<List<LogEntregableDto>> GetHistorialEntregablesByCedula(int cedula)
        {
            var historial = await _logs.GetHistorialEntregablesByCedula(cedula);

            foreach (var h in historial)
            {
                h.Estatus = await _estatus.GetEEByIdAsync(h.EstatusId);
                h.Usuario = await _usuarios.GetUsuarioByIdAsync(h.UsuarioId);
                h.Entregable = await _centregables.GetEntregableById(h.EntregableId);
            }

            return historial;
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
