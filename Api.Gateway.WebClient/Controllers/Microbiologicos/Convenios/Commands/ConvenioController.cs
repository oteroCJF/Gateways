﻿using Api.Gateway.Models.Convenios.Commands;
using Api.Gateway.Models.Convenios.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Limpieza.Convenios;
using Api.Gateway.Proxies.Limpieza.EntregablesContratacion;
using Api.Gateway.Proxies.Limpieza.Variables;
using Api.Gateway.Proxies.Microbiologicos.Convenios.Commands;
using Api.Gateway.Proxies.Microbiologicos.EntregablesContrato.Commands;
using Api.Gateway.Proxies.Microbiologicos.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Microbiologicos.Convenios.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("microbiologicos/convenios")]
    public class ConvenioController : ControllerBase
    {
        private readonly ICConvenioMicrobiologicosProxy _convenios;

        public ConvenioController(ICConvenioMicrobiologicosProxy convenios)
        {
            _convenios = convenios;
        }

        [Route("createConvenio")]
        [HttpPost]
        public async Task<IActionResult> CreateConvenio([FromBody] ConvenioCreateCommand contrato)
        {
            int success = await _convenios.CreateConvenio(contrato);
            return Ok(success);
        }

        [Route("updateConvenio")]
        [HttpPut]
        public async Task<IActionResult> UpdateConvenio([FromBody] ConvenioUpdateCommand contrato)
        {
            int success = await _convenios.UpdateConvenio(contrato);
            return Ok(success);
        }

        [Route("deleteConvenio")]
        [HttpPut]
        public async Task<IActionResult> DeleteConvenio([FromBody] ConvenioDeleteCommand contrato)
        {
            int success = await _convenios.DeleteConvenio(contrato);
            return Ok(success);
        }
    }
}
