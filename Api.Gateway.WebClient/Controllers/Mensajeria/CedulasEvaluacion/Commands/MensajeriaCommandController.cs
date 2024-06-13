using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion;
using Api.Gateway.Proxies.Mensajeria.SoportePago.Queries;
using Api.Gateway.WebClient.Controllers.Mensajeria.CedulasEvaluacion.Procedures;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion.Commands;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using System.Linq;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Procedures.Queries;
using Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Procedures.Commands;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.CedulasEvaluacion.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/cedulaEvaluacion")]
    public class MensajeriaCommandController : ControllerBase
    {
        private readonly IQCedulaMensajeriaProxy _cedulaQuery;
        private readonly IQSoportePagoMensajeriaProxy _soporteQuery;
        private readonly ICCedulaMensajeriaProxy _cedulaCommand;
        private readonly ICedulaMensajeriaProcedure _cedulaProcedure;
        private readonly ICEntregableMensajeriaProcedure _entregablePC;
        private readonly IQMensajeriaEntregableProcedure _entregablePQ;

        public MensajeriaCommandController(IQCedulaMensajeriaProxy cedulaQuery, IQSoportePagoMensajeriaProxy soporteQuery, 
                                           ICCedulaMensajeriaProxy cedulaCommand, ICedulaMensajeriaProcedure cedulaProcedure,
                                           ICEntregableMensajeriaProcedure entregablePC, IQMensajeriaEntregableProcedure entregablePQ)
        {
            _cedulaQuery = cedulaQuery;
            _soporteQuery = soporteQuery;
            _cedulaCommand = cedulaCommand;
            _cedulaProcedure = cedulaProcedure;
            _entregablePC = entregablePC;
            _entregablePQ = entregablePQ;
        }

        [Route("updateCedula")]
        [HttpPut]
        public async Task<IActionResult> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand request)
        {
            var cedula = await _cedulaQuery.GetCedulaById(request.Id);
            List<MSoportePagoDto> guiasPendientes = await _soporteQuery.GetGuiasPendientes(request.Id);
            if (guiasPendientes.Count() == 0 && request.Calcula)
            {
                var command = await _cedulaProcedure.EnviarCedulaEvaluacion(request, cedula);
                cedula = await _cedulaCommand.EnviarCedula(command);
            }
            else if (guiasPendientes.Count() != 0)
            {
                var command = await _cedulaProcedure.DBloquearCedulaEvaluacion(request, cedula);
                cedula = await _cedulaCommand.DBloquearCedula(command);
            }
            else
            {
                cedula = await _cedulaCommand.UpdateCedula(request);
            }

            if (request.Elimina)
            {
                var eliminarEntregables = await _entregablePC.EliminarEntregablesByEC(request);
            }
            else
            {
                var actualizarEntregables = await _entregablePC.ActualizarEntregablesByEC(request);
            }
            var insertarEntregables = await _entregablePC.InsertarEntregablesByEC(request);

            return Ok(cedula);
        }
    }
}
