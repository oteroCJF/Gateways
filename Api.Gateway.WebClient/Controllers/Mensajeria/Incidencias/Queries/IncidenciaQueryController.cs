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

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Incidencias.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/incidenciasCedula")]
    public class IncidenciaQueryController : ControllerBase
    {
        private readonly IQIncidenciaMensajeriaProxy _incidencias;
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly IQCedulaMensajeriaProxy _cedulas;
        private readonly IMesProxy _mes;

        public IncidenciaQueryController(IQIncidenciaMensajeriaProxy incidencias, ICTIncidenciaProxy cincidencias, IMesProxy mes, IQCedulaMensajeriaProxy cedulas)
        {
            _incidencias = incidencias;
            _cincidencias = cincidencias;
            _mes = mes;
            _cedulas = cedulas;
        }

        [Route("getIncidenciasByCedula/{cedula}")]
        public async Task<List<MIncidenciaDto>> GetIncidenciasByCedula(int cedula)
        {
            var incidencias = await _incidencias.GetIncidenciasByCedula(cedula);

            return incidencias;
        }
        
        [Route("getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<List<MIncidenciaDto>> GetIncidenciasByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencias = await _incidencias.GetIncidenciasByPreguntaAndCedula(cedula, pregunta);

            return incidencias;
        }


        [Route("getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<MIncidenciaDto> GetIncidenciaByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencia = await _incidencias.GetIncidenciaByPreguntaAndCedula(cedula, pregunta);

            return incidencia;
        }


        [Route("getConfiguracionIncidencias")]
        public async Task<List<MConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            var incidencia = await _incidencias.GetConfiguracionIncidencias();

            return incidencia;
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
