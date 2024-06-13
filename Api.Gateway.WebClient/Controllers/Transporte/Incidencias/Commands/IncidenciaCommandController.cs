using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Transporte.Commands;
using Api.Gateway.Models.Incidencias.Transporte.DTOs;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Transporte.CedulasEvaluacion;
using Api.Gateway.Proxies.Transporte.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Transporte.Incidencias.Commands;
using Api.Gateway.Proxies.Transporte.Incidencias.Queries;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Transporte.Incidencias.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("transporte/incidenciasCedula")]
    public class IncidenciaCommandController : ControllerBase
    {
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly ICIncidenciaTransporteProxy _incidencias;
        private readonly IMesProxy _mes;
        private readonly IQCedulaTransporteProxy _cedulas;

        public IncidenciaCommandController(ICTIncidenciaProxy cincidencias, ICIncidenciaTransporteProxy incidencias, IQCedulaTransporteProxy cedulas, 
                                           IMesProxy mes)
        {
            _cincidencias = cincidencias;
            _incidencias = incidencias;
            _cedulas = cedulas;
            _mes = mes;
        }

        [Route("insertaIncidencia")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TIncidenciaCreateCommand incidencia)
        {
            await _incidencias.CreateIncidencia(incidencia);
            return Ok();
        }

        [Route("actualizarIncidencia")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TIncidenciaUpdateCommand incidencia)
        {
            await _incidencias.UpdateIncidencia(incidencia);
            return Ok();
        }

        [Route("eliminarIncidencias")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] TIncidenciaDeleteCommand incidencia)
        {
            int incidencias = await _incidencias.DeleteIncidencias(incidencia);

            return Ok(incidencias);
        }

        [Route("eliminarIncidencia")]
        [HttpPost]

        public async Task<IActionResult> DeleteIncidencia([FromBody] TIncidenciaDeleteCommand incidencia)
        {
            int incidencias = await _incidencias.DeleteIncidencia(incidencia);

            return Ok(incidencias);
        }
    }
}
