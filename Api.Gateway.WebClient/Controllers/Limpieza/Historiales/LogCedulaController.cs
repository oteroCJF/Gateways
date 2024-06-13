using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.Historiales;
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
    [Route("limpieza/logCedulas")]
    public class LogCedulaController : ControllerBase
    {
        private readonly ILLogCedulaProxy _logs;
        private readonly IEstatusCedulaProxy _estatus;
        private readonly IUsuarioProxy _usuarios;

        public LogCedulaController(ILLogCedulaProxy logs, IEstatusCedulaProxy estatus, IUsuarioProxy usuarios)
        {
            _logs = logs;
            _estatus = estatus;
            _usuarios = usuarios;
        }

        [HttpGet]
        [Route("getHistorialByCedula/{cedula}")]
        public async Task<List<LogCedulaDto>> GetHistorialByCedula(int cedula)
        {
            var historial = await _logs.GetHistorialByCedula(cedula);

            foreach (var h in historial)
            {
                h.Estatus = await _estatus.GetECByIdAsync(h.EstatusId);
                h.Usuario = await _usuarios.GetUsuarioByIdAsync(h.UsuarioId);
            }
            return historial;
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
