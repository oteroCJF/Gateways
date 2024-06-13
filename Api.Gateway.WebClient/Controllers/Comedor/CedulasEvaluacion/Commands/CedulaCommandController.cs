using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Queries;
using Api.Gateway.WebClient.Controllers.Comedor.CedulasEvaluacion.Procedures;
using Api.Gateway.WebClient.Controllers.Comedor.Entregables.Procedures.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.CedulasEvaluacion.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/cedulaEvaluacion")]
    public class ComedorController : ControllerBase
    {
        private readonly IQCedulaComedorProxy _cedulaQuery;
        private readonly ICCedulaComedorProxy _cedulaCommand;
        private readonly ICedulaComedorProcedure _cedulaProcedure;
        private readonly ICEntregableComedorProcedure _entregablePC;

        public ComedorController(IQCedulaComedorProxy cedulaQuery, ICCedulaComedorProxy cedulaCommand, ICedulaComedorProcedure cedulaProcedure, 
                                 ICEntregableComedorProcedure entregablePC)
        {
            _cedulaQuery = cedulaQuery;
            _cedulaCommand = cedulaCommand;
            _cedulaProcedure = cedulaProcedure;
            _entregablePC = entregablePC;
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
