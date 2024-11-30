using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Estatus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Comedor.Entregables.Commands;
using Api.Gateway.Proxies.Comedor.Entregables.Queries;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using Api.Gateway.WebClient.Controllers.Comedor.Entregables.Procedures.Queries;

namespace Api.Gateway.WebClient.Controllers.Comedor.Entregables.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/entregablesCedula")]
    public class EntregableCommandController : ControllerBase
    {
        private readonly ICEntregableComedorProxy _entregablesc;
        private readonly IQEntregableComedorProxy _entregablesq;
        private readonly IEstatusEntregableProxy _estatus;
        private readonly IQComedorEntregableProcedure _centregable;

        public EntregableCommandController(ICEntregableComedorProxy entregablesc, IQEntregableComedorProxy entregablesq, IEstatusEntregableProxy estatus,
                                           IQComedorEntregableProcedure centregable)
        {
            _entregablesq = entregablesq;
            _entregablesc = entregablesc;
            _estatus = estatus;
            _centregable = centregable;
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPut("actualizarEntregable")]
        public async Task<IActionResult> ActualizaEntregable([FromForm] EntregableCommandUpdate request)
        {
            var entregable = await _entregablesq.GetEntregableById(request.Id);
            if ((await _estatus.GetEEByIdAsync(entregable.EstatusId)).Nombre.Equals("Rechazado") || 
                (await _estatus.GetEEByIdAsync(entregable.EstatusId)).Nombre.Equals("Sin Iniciar"))
            {
                request.EstatusId = (await _estatus.GetAllEstatusEntregablesAsync()).SingleOrDefault(e => e.Nombre.Equals("En Proceso")).Id;
            }
            await _entregablesc.UpdateEntregable(request);
            return Ok();

        }

        [HttpPut]
        [Route("AREntregable")]
        public async Task<IActionResult> AREntregable([FromForm] EEntregableUpdateCommand entregable)
        {
            entregable.EstatusId = (await _estatus.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals(entregable.Estatus)).Id;
            entregable.FechaActualizacion = DateTime.Now;
            await _entregablesc.AUpdateEntregable(entregable);
            return Ok();

        }

        [Route("descargarEntregables")]
        [HttpPost]
        public async Task<string> DescargarEntregables([FromBody] DEntregablesCommand request)
        {
            request.Path = await _entregablesq.GetPathEntregables();
            var entregables = await _centregable.DescargarEntregables(request);
            return entregables;
        }

        [HttpPut("validarEntregable")]
        public async Task<IActionResult> ValidarEntregable([FromForm] EntregableCommandUpdate request)
        {
            await _entregablesc.UpdateEntregable(request);
            return Ok();
        }
    }
}
