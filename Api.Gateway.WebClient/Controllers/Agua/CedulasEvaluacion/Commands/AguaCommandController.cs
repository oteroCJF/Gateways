using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Agua.CedulasEvaluacion;
using Api.Gateway.WebClient.Controllers.Agua.CedulasEvaluacion.Procedures;
using Api.Gateway.Proxies.Agua.CedulasEvaluacion.Commands;
using System.Linq;
using Api.Gateway.WebClient.Controllers.Agua.Entregables.Procedures.Queries;
using Api.Gateway.WebClient.Controllers.Agua.Entregables.Procedures.Commands;

namespace Api.Gateway.WebClient.Controllers.Agua.CedulasEvaluacion.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/cedulaEvaluacion")]
    public class AguaCommandController : ControllerBase
    {
        private readonly IQCedulaAguaProxy _cedulaQuery;
        private readonly ICCedulaAguaProxy _cedulaCommand;
        private readonly ICedulaAguaProcedure _cedulaProcedure;
        private readonly ICEntregableAguaProcedure _entregablePC;
        private readonly IQAguaEntregableProcedure _entregablePQ;

        public AguaCommandController(IQCedulaAguaProxy cedulaQuery, ICCedulaAguaProxy cedulaCommand, ICedulaAguaProcedure cedulaProcedure,
                                           ICEntregableAguaProcedure entregablePC, IQAguaEntregableProcedure entregablePQ)
        {
            _cedulaQuery = cedulaQuery;
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
            if (request.Calcula)
            {
                var command = await _cedulaProcedure.EnviarCedulaEvaluacion(request, cedula);
                cedula = await _cedulaCommand.EnviarCedula(command);
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
