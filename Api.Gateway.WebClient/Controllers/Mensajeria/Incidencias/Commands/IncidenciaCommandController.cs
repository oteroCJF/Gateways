using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Mensajeria.Commands;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Mensajeria.Incidencias.Commands;
using Api.Gateway.Proxies.Mensajeria.Incidencias.Queries;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Incidencias.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/incidenciasCedula")]
    public class IncidenciaCommandController : ControllerBase
    {
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly ICIncidenciaMensajeriaProxy _incidencias;
        private readonly IMesProxy _mes;
        private readonly IQCedulaMensajeriaProxy _cedulas;

        public IncidenciaCommandController(ICTIncidenciaProxy cincidencias, ICIncidenciaMensajeriaProxy incidencias, IQCedulaMensajeriaProxy cedulas, 
                                           IMesProxy mes)
        {
            _cincidencias = cincidencias;
            _incidencias = incidencias;
            _cedulas = cedulas;
            _mes = mes;
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [Route("insertaIncidencia")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MIncidenciaCreateCommand incidencia)
        {
            incidencia.TipoIncidencia = (await _cincidencias.GetIncidenciaById(incidencia.IncidenciaId)).Abreviacion;
            if (incidencia.Comprobante != null || incidencia.Escrito != null || incidencia.Acta != null)
            {
                var cedula = await _cedulas.GetCedulaById(incidencia.CedulaEvaluacionId);
                incidencia.Mes = (await _mes.GetMesByIdAsync(cedula.MesId)).Nombre;
                incidencia.Anio = cedula.Anio;
                incidencia.Folio = cedula.Folio;
            }
            await _incidencias.CreateIncidencia(incidencia);
            return Ok();
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [Route("actualizarIncidencia")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] MIncidenciaUpdateCommand incidencia)
        {
            incidencia.TipoIncidencia = (await _cincidencias.GetIncidenciaById(incidencia.IncidenciaId)).Abreviacion;
            if (incidencia.Comprobante != null || incidencia.Escrito != null || incidencia.Acta != null)
            {
                var cedula = await _cedulas.GetCedulaById(incidencia.CedulaEvaluacionId);
                incidencia.Mes = (await _mes.GetMesByIdAsync(cedula.MesId)).Nombre;
                incidencia.Anio = cedula.Anio;
                incidencia.Folio = cedula.Folio;
            }
            await _incidencias.UpdateIncidencia(incidencia);
            return Ok();
        }

        [Route("eliminarIncidencias")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] MIncidenciaDeleteCommand incidencia)
        {
            var cedulaE = await _cedulas.GetCedulaById(incidencia.CedulaEvaluacionId);
            incidencia.Mes = (await _mes.GetMesByIdAsync(cedulaE.MesId)).Nombre;
            incidencia.Anio = cedulaE.Anio;
            incidencia.Folio = cedulaE.Folio;
            int incidencias = await _incidencias.DeleteIncidencias(incidencia);

            return Ok(incidencias);
        }

        [Route("eliminarIncidencia")]
        [HttpPost]

        public async Task<IActionResult> DeleteIncidencia([FromBody] MIncidenciaDeleteCommand incidencia)
        {
            var cedulaE = await _cedulas.GetCedulaById(incidencia.CedulaEvaluacionId);
            incidencia.Mes = (await _mes.GetMesByIdAsync(cedulaE.MesId)).Nombre;
            incidencia.Anio = cedulaE.Anio;
            incidencia.Folio = cedulaE.Folio;
            int incidencias = await _incidencias.DeleteIncidencia(incidencia);

            return Ok(incidencias);
        }

        [Consumes("multipart/form-data")]
        [Route("insertaIncidenciaExcel")]
        [HttpPost]
        public async Task<IActionResult> InsertaIncidenciaExcel([FromForm] MIncidenciaExcelCreateCommand incidencia)
        {
            var incidencias = await _incidencias.CreateIncidenciaExcel(incidencia);
            return Ok(incidencias);
        }
    }
}
