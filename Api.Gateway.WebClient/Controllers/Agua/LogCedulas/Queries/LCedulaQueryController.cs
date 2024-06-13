using Api.Gateway.Models.LogsCedulas.Commands;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.Historiales;
using Api.Gateway.Proxies.Agua.LogCedulas.Commands;
using Api.Gateway.Proxies.Agua.LogCedulas.Queries;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.LogCedulas.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/logCedulas")]
    public class LCedulaQueryController : ControllerBase
    {
        private readonly IQLCedulaAguaProxy _logs;
        private readonly IEstatusCedulaProxy _estatus;
        private readonly IUsuarioProxy _usuarios;

        public LCedulaQueryController(IQLCedulaAguaProxy logs, IEstatusCedulaProxy estatus, IUsuarioProxy usuarios)
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
