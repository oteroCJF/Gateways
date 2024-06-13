using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Transporte.CedulasEvaluacion;
using Api.Gateway.WebClient.Controllers.Transporte.CedulasEvaluacion.Procedures;
using Api.Gateway.Proxies.Transporte.CedulasEvaluacion.Commands;
using Api.Gateway.WebClient.Controllers.Transporte.Entregables.Procedures.Queries;
using Api.Gateway.WebClient.Controllers.Transporte.Entregables.Procedures.Commands;

namespace Api.Gateway.WebClient.Controllers.Transporte.CedulasEvaluacion.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("transporte/cedulaEvaluacion")]
    public class TransporteCommandController : ControllerBase
    {
        private readonly IQCedulaTransporteProxy _cedulaQuery;
        private readonly ICCedulaTransporteProxy _cedulaCommand;
        private readonly ICedulaTransporteProcedure _cedulaProcedure;
        private readonly ICEntregableTransporteProcedure _entregablePC;
        private readonly IQTransporteEntregableProcedure _entregablePQ;

        public TransporteCommandController(IQCedulaTransporteProxy cedulaQuery, 
                                           ICCedulaTransporteProxy cedulaCommand, ICedulaTransporteProcedure cedulaProcedure,
                                           ICEntregableTransporteProcedure entregablePC, IQTransporteEntregableProcedure entregablePQ)
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
            cedula = await _cedulaCommand.UpdateCedula(request);

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
