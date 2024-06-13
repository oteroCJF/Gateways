﻿using Api.Gateway.Models.Incidencias.Limpieza.Commands;
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

namespace Api.Gateway.WebClient.Controllers.Comedor.Incidencias.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/incidenciasCedula")]
    public class IncidenciaQueryController : ControllerBase
    {
        private readonly IQIncidenciaComedorProxy _incidencias;
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly IQCedulaComedorProxy _cedulas;
        private readonly IMesProxy _mes;

        public IncidenciaQueryController(IQIncidenciaComedorProxy incidencias, ICTIncidenciaProxy cincidencias, IMesProxy mes, IQCedulaComedorProxy cedulas)
        {
            _incidencias = incidencias;
            _cincidencias = cincidencias;
            _mes = mes;
            _cedulas = cedulas;
        }

        [Route("getIncidenciasByCedula/{cedula}")]
        public async Task<List<CIncidenciaDto>> GetIncidenciasByCedula(int cedula)
        {
            var incidencias = await _incidencias.GetIncidenciasByCedula(cedula);

            return incidencias;
        }
        
        [Route("getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<List<CIncidenciaDto>> GetIncidenciasByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencias = await _incidencias.GetIncidenciasByPreguntaAndCedula(cedula, pregunta);

            return incidencias;
        }


        [Route("getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}")]
        public async Task<CIncidenciaDto> GetIncidenciaByCedulaAndPregunta(int cedula, int pregunta)
        {
            var incidencia = await _incidencias.GetIncidenciaByPreguntaAndCedula(cedula, pregunta);

            return incidencia;
        }


        [Route("getConfiguracionIncidencias")]
        public async Task<List<CConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
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
