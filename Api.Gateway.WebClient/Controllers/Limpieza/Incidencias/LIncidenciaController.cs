using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using Api.Gateway.Proxies.Limpieza.Incidencias;
using Api.Gateway.Proxies.Limpieza.ServicioContrato;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.Incidencias
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/incidenciasCedula")]
    public class LIncidenciaController : ControllerBase
    {
        private readonly ILIncidenciaProxy _incidencias;
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly ILServicioContratoProxy _scontratos;
        private readonly ICTServicioContratoProxy _cscontratos;
        private readonly ILCedulaProxy _cedulas;
        private readonly IMesProxy _mes;

        public LIncidenciaController(ILIncidenciaProxy incidencias, ICTIncidenciaProxy cincidencias, ILServicioContratoProxy scontratos, ICTServicioContratoProxy cscontratos, ILCedulaProxy cedulas, 
                                     IMesProxy mes)
        {
            _incidencias = incidencias;
            _cincidencias = cincidencias;
            _scontratos = scontratos;
            _cscontratos = cscontratos;
            _cedulas = cedulas;
            _mes = mes;
        }

        [Route("getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<List<LIncidenciaDto>> GetIncidenciasByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencias = await _incidencias.GetIncidenciasByPreguntaAndCedula(cedula, pregunta);

            return incidencias;
        }

        [Route("getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<LIncidenciaDto> GetIncidenciaByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencia = await _incidencias.GetIncidenciaByPreguntaAndCedula(cedula, pregunta);

            return incidencia;
        }

        [Route("getConfiguracionIncidencias")]
        public async Task<List<LConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            var incidencia = await _incidencias.GetConfiguracionIncidencias();

            return incidencia;
        }

        [Route("insertaIncidencia")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LIncidenciaCreateCommand request)
        {
            var cedula = await _cedulas.GetCedulaById(request.CedulaEvaluacionId);
            request.Penalizacion = await _scontratos.GetServiciosByContrato(cedula.ContratoId);

            foreach (var sc in request.Penalizacion)
            {
                sc.Servicio = await _cscontratos.GetServicioContratoByIdAsync(sc.ServicioId);
            }

            await _incidencias.CreateIncidencia(request);
            return Ok();
        }

        [Route("actualizarIncidencia")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LIncidenciaUpdateCommand request)
        {
            var cedula = await _cedulas.GetCedulaById(request.CedulaEvaluacionId);
            request.Penalizacion = await _scontratos.GetServiciosByContrato(cedula.ContratoId);

            foreach (var sc in request.Penalizacion)
            {
                sc.Servicio = await _cscontratos.GetServicioContratoByIdAsync(sc.ServicioId);
            }
            await _incidencias.UpdateIncidencia(request);
            return Ok();
        }

        [Route("eliminarIncidencias")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] LIncidenciaDeleteCommand incidencia)
        {
            int incidencias = await _incidencias.DeleteIncidencias(incidencia);

            return Ok(incidencias);
        }

        [Route("eliminarIncidencia")]
        [HttpPost]

        public async Task<IActionResult> DeleteIncidencia([FromBody] LIncidenciaDeleteCommand incidencia)
        {
            int incidencias = await _incidencias.DeleteIncidencia(incidencia);

            return Ok(incidencias);
        }

        [Route("visualizarActas/{anio}/{mes}/{folio}/{tipo}/{tipoArchivo}/{archivo}")]
        [HttpGet]
        public async Task<string> VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo)
        {
            var file = await _incidencias.VisualizarActas(anio, mes, folio, tipo, tipoArchivo, archivo);

            return file;
        }
    }
}
