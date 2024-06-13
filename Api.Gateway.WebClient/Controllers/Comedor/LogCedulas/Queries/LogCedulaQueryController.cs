using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Comedor.LogCedula.Queries;

namespace Api.Gateway.WebClient.Controllers.Comedor.LogCedulas.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/logCedulas")]
    public class LogCedulaQueryController : ControllerBase
    {
        private readonly IQLCedulaComedorProxy _logs;
        private readonly IEstatusCedulaProxy _estatus;
        private readonly IUsuarioProxy _usuarios;

        public LogCedulaQueryController(IQLCedulaComedorProxy logs, IEstatusCedulaProxy estatus, IUsuarioProxy usuarios)
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
    }
}
