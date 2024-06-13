using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Comedor;
using Api.Gateway.Models.Cuestionarios.DTOs;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Comedor.Cuestionarios.Queries;
using Api.Gateway.Proxies.Comedor.Incidencias.Queries;
using Api.Gateway.Proxies.Comedor.Respuestas.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.CedulasEvaluacion.Procedures
{
    public interface ICuestionarioComedorProcedure
    {
        Task<List<CuestionarioComedorDto>> GetCuestionarioComedor(CedulaComedorDto cedula); 
    }
    
    public class CuestionarioComedorProcedure : ICuestionarioComedorProcedure
    {
        private readonly IQCuestionarioComedorProxy _cuestionarios;
        private readonly IQRespuestaComedorProxy _respuestas;
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly ICTIComedorProxy _ctiComedor;
        private readonly IQIncidenciaComedorProxy _incidenciasQuery;
        private readonly ICTParametroProxy _parametros;

        public CuestionarioComedorProcedure(IQCuestionarioComedorProxy cuestionarios, IQRespuestaComedorProxy respuestas, ICTIncidenciaProxy cincidencias,
                                            IQIncidenciaComedorProxy incidenciasQuery, ICTParametroProxy parametros,
                                            ICTIComedorProxy ctiComedor)
        {
            _cuestionarios = cuestionarios;
            _cincidencias = cincidencias;
            _respuestas = respuestas;
            _incidenciasQuery = incidenciasQuery;
            _parametros = parametros;
            _ctiComedor = ctiComedor;
        }

        public async Task<List<CuestionarioComedorDto>> GetCuestionarioComedor(CedulaComedorDto cedula)
        {
            List<CuestionarioComedorDto> cuestionario = new List<CuestionarioComedorDto>();
            List<int> categoriasId = (await _cuestionarios.GetCuestionarioMensualId(cedula.Anio, cedula.MesId, cedula.ContratoId, cedula.ServicioId)).Select(cm => cm.CategoriaId).Distinct().ToList();

            List<CTParametroDto> categorias = (await _parametros.GetAllParametrosAsync()).Where(p => categoriasId.Contains(p.Id)).OrderBy(p => p.Orden).ToList();

            foreach (var ct in categorias)
            {
                var cs = new CuestionarioComedorDto();
                cs.Id = ct.Id;
                cs.Tipo = ct.Tipo;
                cs.Tabla = ct.Tabla;
                cs.Abreviacion = ct.Abreviacion;
                cs.Nombre = ct.Nombre;
                cs.Respuestas = await GetRespuestasCategoria(cedula, ct.Id);
                cuestionario.Add(cs);
            }

            return cuestionario;
        }

        public async Task<List<CRespuestaDto>> GetRespuestasCategoria(CedulaComedorDto cedula, int categoria)
        {
            try
            {
                List<int> preguntas = (await _cuestionarios.GetCuestionarioMensualId(cedula.Anio, cedula.MesId, cedula.ContratoId, cedula.ServicioId))
                                                    .Where(cm => cm.CategoriaId == categoria).Select(cm => cm.Consecutivo).ToList();

                IEnumerable<CuestionarioMensualDto> cuestionarioMensual = (await _cuestionarios.GetCuestionarioMensualId(cedula.Anio, cedula.MesId, cedula.ContratoId, cedula.ServicioId)).OrderBy(c => c.Consecutivo);

                List<CRespuestaDto> respuestas = (await _respuestas.GetRespuestasEvaluacionByCedulaAnioMes(cedula.Id)).Where(r => preguntas.Contains(r.Pregunta)).ToList();

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
                    //dt.cuestionario.IncidenciaId = (await _cincidencias.GetAllIncidenciasAsync()).Single(i => i.Abreviacion.Equals(dt.cuestionario.Abreviacion)).Id;
                    dt.iComedor = await _incidenciasQuery.GetIncidenciasByPreguntaAndCedula(dt.CedulaEvaluacionId, dt.cuestionario.Consecutivo);
                    if (dt.Respuesta != null)
                    {
                        dt.ciComedor = await _incidenciasQuery.GetConfiguracionIncidenciasByPregunta(dt.cuestionario.Id, (bool)dt.Respuesta);
                        foreach (var cn in dt.iComedor)
                        {
                            cn.ciComedor = await _incidenciasQuery.GetConfiguracionIncidenciasByPregunta(dt.cuestionario.Id, (bool)dt.Respuesta);
                            if (cn.IncidenciaId != 0)
                            {
                                cn.DIncidencias = await _incidenciasQuery.GetDetalleIncidenciasById(cn.Id);
                                cn.Incidencia = await _cincidencias.GetIncidenciaById(cn.IncidenciaId);
                                if (cn.Ponderacion != 0)
                                {
                                    var parametros = await _parametros.GetParametroByTipo("PlantillaComedor");
                                    cn.Perfil = parametros.Single(p => p.Orden == cn.Ponderacion);
                                }
                                foreach (var dtic in cn.DIncidencias)
                                {
                                    dtic.DIncidencia = await _ctiComedor.GetIncidenciaById(dtic.CIncidenciaId);
                                    dtic.DIncidencia.Tipo = await _parametros.GetParametroById(dtic.DIncidencia.TipoId);
                                    
                                }
                            }
                        }
                    }
                }

                return respuestas;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
