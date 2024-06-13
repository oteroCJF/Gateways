using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Agua.Commands;
using Api.Gateway.Models.Incidencias.Agua.DTOs;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Agua.CedulasEvaluacion;
using Api.Gateway.Proxies.Agua.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Agua.Incidencias.Commands;
using Api.Gateway.Proxies.Agua.Incidencias.Queries;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.Incidencias.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/incidenciasCedula")]
    public class IncidenciaCommandController : ControllerBase
    {
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly ICIncidenciaAguaProxy _incidencias;
        private readonly IMesProxy _mes;
        private readonly IQCedulaAguaProxy _cedulas;

        public IncidenciaCommandController(ICTIncidenciaProxy cincidencias, ICIncidenciaAguaProxy incidencias, IQCedulaAguaProxy cedulas, 
                                           IMesProxy mes)
        {
            _cincidencias = cincidencias;
            _incidencias = incidencias;
            _cedulas = cedulas;
            _mes = mes;
        }

        [Route("insertaIncidencia")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AIncidenciaCreateCommand incidencia)
        {
            await _incidencias.CreateIncidencia(incidencia);
            return Ok();
        }

        [Route("actualizarIncidencia")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AIncidenciaUpdateCommand incidencia)
        {
            await _incidencias.UpdateIncidencia(incidencia);
            return Ok();
        }

        [Route("eliminarIncidencias")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] AIncidenciaDeleteCommand incidencia)
        {
            var cedulaE = await _cedulas.GetCedulaById(incidencia.CedulaEvaluacionId);
            int incidencias = await _incidencias.DeleteIncidencias(incidencia);

            return Ok(incidencias);
        }

        [Route("eliminarIncidencia")]
        [HttpPost]

        public async Task<IActionResult> DeleteIncidencia([FromBody] AIncidenciaDeleteCommand incidencia)
        {
            var cedulaE = await _cedulas.GetCedulaById(incidencia.CedulaEvaluacionId);
            int incidencias = await _incidencias.DeleteIncidencia(incidencia);

            return Ok(incidencias);
        }
    }
}
