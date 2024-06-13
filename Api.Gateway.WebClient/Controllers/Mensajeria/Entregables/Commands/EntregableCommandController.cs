﻿using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Mensajeria.Entregables;
using Api.Gateway.Proxies.Mensajeria.Entregables.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/entregablesCedula")]
    public class EntregableCommandController : ControllerBase
    {
        private readonly ICEntregableMensajeriaProxy _entregablesCommand;
        private readonly IQEntregableMensajeriaProxy _entregablesQuery;
        private readonly IEstatusEntregableProxy _estatus;

        public EntregableCommandController(ICEntregableMensajeriaProxy entregablesCommand, IEstatusEntregableProxy estatus, 
                                           IQEntregableMensajeriaProxy entregablesQuery)
        {
            _entregablesQuery= entregablesQuery;
            _entregablesCommand = entregablesCommand;
            _estatus = estatus;
        }


        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPut("actualizarEntregable")]
        public async Task<IActionResult> ActualizaEntregable([FromForm] EntregableCommandUpdate request)
        {
            var entregable = await _entregablesQuery.GetEntregableById(request.Id);
            var estatusEntregable = await _estatus.GetEEntregableByEC(request.EstatusId, entregable.EntregableId, request.Supervicion);
            request.EstatusId = estatusEntregable.EEstatusId != 0 ? estatusEntregable.EEstatusId : request.EstatusId;
            await _entregablesCommand.UpdateEntregable(request);
            return Ok();

        }
        
        [HttpPut("validarEntregable")]
        public async Task<IActionResult> ValidarEntregable([FromForm] EntregableCommandUpdate request)
        {
            await _entregablesCommand.UpdateEntregable(request);
            return Ok();
        }



        [HttpPut]
        [Route("AREntregable")]
        public async Task<IActionResult> AREntregable([FromForm] EEntregableUpdateCommand entregable)
        {
            entregable.EstatusId = (await _estatus.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals(entregable.Estatus)).Id;
            entregable.UsuarioId = entregable.UsuarioId;
            entregable.FechaActualizacion = DateTime.Now;
            await _entregablesCommand.AUpdateEntregable(entregable);
            return Ok();

        }
    }

}
