using Api.Gateway.Models.Incidencias.Agua.DTOs;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Agua.CedulasEvaluacion;
using Api.Gateway.Proxies.Agua.Incidencias.Queries;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.Incidencias.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/incidenciasCedula")]
    public class IncidenciaQueryController : ControllerBase
    {
        private readonly IQIncidenciaAguaProxy _incidencias;
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly IQCedulaAguaProxy _cedulas;
        private readonly IMesProxy _mes;

        public IncidenciaQueryController(IQIncidenciaAguaProxy incidencias, ICTIncidenciaProxy cincidencias, IMesProxy mes, IQCedulaAguaProxy cedulas)
        {
            _incidencias = incidencias;
            _cincidencias = cincidencias;
            _mes = mes;
            _cedulas = cedulas;
        }

        [Route("getIncidenciasByCedula/{cedula}")]
        public async Task<List<AIncidenciaDto>> GetIncidenciasByCedula(int cedula)
        {
            var incidencias = await _incidencias.GetIncidenciasByCedula(cedula);

            return incidencias;
        }
        
        [Route("getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<List<AIncidenciaDto>> GetIncidenciasByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencias = await _incidencias.GetIncidenciasByPreguntaAndCedula(cedula, pregunta);

            return incidencias;
        }


        [Route("getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<AIncidenciaDto> GetIncidenciaByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencia = await _incidencias.GetIncidenciaByPreguntaAndCedula(cedula, pregunta);

            return incidencia;
        }


        [Route("getConfiguracionIncidencias")]
        public async Task<List<AConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
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
