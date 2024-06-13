using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Comedor.Commands;
using Api.Gateway.Models.Incidencias.Comedor.DTOs;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Comedor.Incidencias.Commands;
using Api.Gateway.Proxies.Comedor.Incidencias.Queries;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Queries;

namespace Api.Gateway.WebClient.Controllers.Comedor.Incidencias.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/incidenciasCedula")]
    public class IncidenciaCommandController : ControllerBase
    {
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly ICIncidenciaComedorProxy _incidencias;
        private readonly IMesProxy _mes;
        private readonly IQCedulaComedorProxy _cedulas;

        public IncidenciaCommandController(ICTIncidenciaProxy cincidencias, ICIncidenciaComedorProxy incidencias, IQCedulaComedorProxy cedulas, 
                                           IMesProxy mes)
        {
            _cincidencias = cincidencias;
            _incidencias = incidencias;
            _cedulas = cedulas;
            _mes = mes;
        }

        [Route("insertaIncidencia")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CIncidenciaCreateCommand incidencia)
        {
            await _incidencias.CreateIncidencia(incidencia);
            return Ok();
        }

        [Route("actualizarIncidencia")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CIncidenciaUpdateCommand incidencia)
        {
            await _incidencias.UpdateIncidencia(incidencia);
            return Ok();
        }

        [Route("eliminarIncidencias")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] CIncidenciaDeleteCommand incidencia)
        {
            var cedulaE = await _cedulas.GetCedulaById(incidencia.CedulaEvaluacionId);
            int incidencias = await _incidencias.DeleteIncidencias(incidencia);

            return Ok(incidencias);
        }

        [Route("eliminarIncidencia")]
        [HttpPost]

        public async Task<IActionResult> DeleteIncidencia([FromBody] CIncidenciaDeleteCommand incidencia)
        {
            var cedulaE = await _cedulas.GetCedulaById(incidencia.CedulaEvaluacionId);
            int incidencias = await _incidencias.DeleteIncidencia(incidencia);

            return Ok(incidencias);
        }

    }
}
