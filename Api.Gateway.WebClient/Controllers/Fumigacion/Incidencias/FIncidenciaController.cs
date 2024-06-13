using Api.Gateway.Models.Incidencias.Fumigacion.Commands;
using Api.Gateway.Models.Incidencias.Fumigacion.DTOs;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Fumigacion.Incidencias;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Fumigacion.Incidencias
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("fumigacion/incidenciasCedula")]
    public class FIncidenciaController : ControllerBase
    {
        private readonly IFIncidenciaProxy _incidencias;
        private readonly ICTIncidenciaProxy _cincidencias;
        public FIncidenciaController(IFIncidenciaProxy incidencias, ICTIncidenciaProxy cincidencias)
        {
            _incidencias = incidencias;
            _cincidencias = cincidencias;
        }

        [Route("getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<List<FIncidenciaDto>> GetIncidenciasByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencias = await _incidencias.GetIncidenciasByPreguntaAndCedula(cedula, pregunta);

            return incidencias;
        }

        [Route("getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<FIncidenciaDto> GetIncidenciaByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencia = await _incidencias.GetIncidenciaByPreguntaAndCedula(cedula, pregunta);

            return incidencia;
        }

        [Route("getConfiguracionIncidencias")]
        public async Task<List<FConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            var incidencia = await _incidencias.GetConfiguracionIncidencias();

            return incidencia;
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [Route("insertaIncidencia")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] FIncidenciaCreateCommand incidencia)
        {
            await _incidencias.CreateIncidencia(incidencia);
            return Ok();
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [Route("actualizarIncidencia")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] FIncidenciaUpdateCommand incidencia)
        {
            await _incidencias.UpdateIncidencia(incidencia);
            return Ok();
        }

        [Route("eliminarIncidencias")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] FIncidenciaDeleteCommand incidencia)
        {
            int incidencias = await _incidencias.DeleteIncidencias(incidencia);

            return Ok(incidencias);
        }

        [Route("eliminarIncidencia")]
        [HttpPost]

        public async Task<IActionResult> DeleteIncidencia([FromBody] FIncidenciaDeleteCommand incidencia)
        {
            int incidencias = await _incidencias.DeleteIncidencia(incidencia);

            return Ok(incidencias);
        }


        [Route("visualizarActas/{anio}/{mes}/{folio}/{tipo}/{tipoArchivo}/{archivo}")]
        [HttpGet]
        public async Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo)
        {
            var file = await _incidencias.VisualizarActas(anio, mes, folio, tipo, tipoArchivo,archivo);

            return file;
        }
    }
}
