using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Comedor;
using Api.Gateway.Models.Cuestionarios.DTOs;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Queries;
using Api.Gateway.Proxies.Comedor.Cuestionarios.Queries;
using Api.Gateway.Proxies.Comedor.Incidencias.Queries;
using Api.Gateway.Proxies.Comedor.Respuestas.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.Respuestas.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/respuestasEvaluacion")]
    public class RespuestaQueryController : ControllerBase
    {
        private readonly IQRespuestaComedorProxy _respuestas;
        private readonly IQCedulaComedorProxy _cedula;
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly IQIncidenciaComedorProxy _incidenciasQuery;
        private readonly IQCuestionarioComedorProxy _cuestionarios;
        private readonly ICTIComedorProxy _ctiComedor;

        public RespuestaQueryController(IQRespuestaComedorProxy respuestas, IQCedulaComedorProxy cedula, ICTIncidenciaProxy cincidencias, 
                                        IQIncidenciaComedorProxy incidenciasQuery, IQCuestionarioComedorProxy cuestionarios, ICTIComedorProxy ctiComedor)
        {
            _respuestas = respuestas;
            _cedula = cedula;
            _cincidencias = cincidencias;
            _incidenciasQuery = incidenciasQuery;
            _cuestionarios = cuestionarios;
            _ctiComedor = ctiComedor;
        }

        [Route("{cedulaId}")]
        [HttpGet]
        public async Task<List<CRespuestaDto>> GetCedulaEvaluacionByCedulaAnioMes(int cedulaId)
        {
            try
            {
                var respuestas = await _respuestas.GetRespuestasEvaluacionByCedulaAnioMes(cedulaId);
                var cedula = await _cedula.GetCedulaById(cedulaId);
                IEnumerable<CuestionarioMensualDto> cuestionarioMensual = (await _cuestionarios.GetCuestionarioMensualId(cedula.Anio, cedula.MesId, cedula.ContratoId, cedula.ServicioId)).OrderBy(c => c.Consecutivo);
                foreach (var dt in respuestas)
                {
                    dt.cuestionario = await _cuestionarios.GetPreguntaById(cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).CuestionarioId);
                    dt.cuestionario.Ponderacion = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Ponderacion;
                    dt.cuestionario.ACLRS = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).ACLRS;
                    dt.cuestionario.Tipo = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Tipo;
                    dt.cuestionario.Formula = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Formula;
                    dt.cuestionario.Porcentaje = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Porcentaje;
                    dt.cuestionario.CategoriaId = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).CategoriaId;
                    dt.cuestionario.Consecutivo = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Consecutivo;
                    dt.iComedor = await _incidenciasQuery.GetIncidenciasByPreguntaAndCedula(dt.CedulaEvaluacionId, dt.cuestionario.Consecutivo);
                    if (dt.Respuesta != null)
                    {
                        dt.ciComedor = await _incidenciasQuery.GetConfiguracionIncidenciasByPregunta(dt.cuestionario.Id, (bool)dt.Respuesta);
                        foreach (var cn in dt.iComedor)
                        {
                            cn.ciComedor = await _incidenciasQuery.GetConfiguracionIncidenciasByPregunta(dt.cuestionario.Id, (bool)dt.Respuesta);
                            if (cn.IncidenciaId != 0)
                            {
                                cn.Incidencia = await _cincidencias.GetIncidenciaById(cn.IncidenciaId);
                                cn.DIncidencias = await _incidenciasQuery.GetDetalleIncidenciasById(cn.Id);
                                foreach (var dtic in cn.DIncidencias)
                                {
                                    dtic.DIncidencia = await _ctiComedor.GetIncidenciaById(dtic.CIncidenciaId);
                                }
                            }
                        }
                    }
                }
                return respuestas;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        [Route("getRespuestasByAnio/{anio}")]
        [HttpGet]
        public async Task<List<CRespuestaDto>> GetAllRespuestasByAnioAsync(int anio)
        {
            var respuestas = await _respuestas.GetAllRespuestasByAnioAsync(anio);

            return respuestas;
        }
    }
}
