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

namespace Api.Gateway.WebClient.Controllers.Mensajeria.LogEntregables.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/logEntregables")]
    public class LEntregableQueryController : ControllerBase
    {
        private readonly IQLEntregableMensajeriaProxy _logs;
        private readonly IEstatusEntregableProxy _estatus;
        private readonly ICTEntregableProxy _centregables;
        private readonly IUsuarioProxy _usuarios;

        public LEntregableQueryController(IQLEntregableMensajeriaProxy logs, IEstatusEntregableProxy estatus, IUsuarioProxy usuarios,
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
    }
}
