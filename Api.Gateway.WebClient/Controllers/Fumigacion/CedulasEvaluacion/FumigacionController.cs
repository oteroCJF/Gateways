using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Fumigacion;
using Api.Gateway.Models.Cuestionarios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Fumigacion.CedulaEvaluacion;
using Api.Gateway.Proxies.Fumigacion.CedulasEvaluacion;
using Api.Gateway.Proxies.Fumigacion.CFDIs;
using Api.Gateway.Proxies.Fumigacion.Contratos;
using Api.Gateway.Proxies.Fumigacion.Cuestionarios;
using Api.Gateway.Proxies.Fumigacion.Entregables;
using Api.Gateway.Proxies.Fumigacion.Facturacion;
using Api.Gateway.Proxies.Fumigacion.Historiales;
using Api.Gateway.Proxies.Fumigacion.Incidencias;
using Api.Gateway.Proxies.Fumigacion.ServicioContrato;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Estatus;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Fumigacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Fumigacion.CedulaEvaluacion
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("fumigacion/cedulaEvaluacion")]
    public class FumigacionController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IUsuarioProxy _usuarios;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly IEstatusEntregableProxy _estatuse;
        private readonly IFCuestionarioProxy _cuestionario;
        private readonly ICTEntregableProxy _centregable;
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly ICTServicioContratoProxy _cscontratos;
        private readonly IFCedulaProxy _cedula;
        private readonly IFRespuestaProxy _respuestas;
        private readonly IFIncidenciaProxy _incidencias;
        private readonly IFEntregableProxy _entregables;
        private readonly IFLCedulaProxy _logs;
        private readonly IFLEntregableProxy _logse;
        private readonly IEstatusProcedure _pestatus;
        private readonly IFServicioContratoProxy _scontratos;
        private readonly IFRepositorioProxy _repositorios;
        private readonly IFCFDIProxy _facturas;
        private readonly IFContratoProxy _contrato;
        private readonly IFCedulaProcedure _fcedula;
        private readonly IVEFumigacionProcedure _veentregables;
        private readonly IVFumigacionProcedure _vfumigacion;        
        private readonly ICTIFumigacionProxy _ctfumigacion;

        public FumigacionController(IMesProxy meses, IInmuebleProxy inmuebles, IUsuarioProxy usuarios, IEstatusCedulaProxy estatusc,
                                    IEstatusEntregableProxy estatuse, IFCuestionarioProxy cuestionario, ICTEntregableProxy centregable,
                                    ICTIncidenciaProxy cincidencias, ICTServicioContratoProxy cscontratos, IFCedulaProxy cedula,
                                    IFRespuestaProxy respuestas, IFIncidenciaProxy incidencias, IFEntregableProxy entregables,
                                    IFLCedulaProxy logs, IFLEntregableProxy logse, IEstatusProcedure pestatus,
                                    IFServicioContratoProxy scontratos, IFRepositorioProxy repositorios, IFCFDIProxy facturas,
                                    IFContratoProxy contrato, IVEFumigacionProcedure veentregables, ICTIFumigacionProxy ctfumigacion,
                                    IVFumigacionProcedure vfumigacion, IFCedulaProcedure fcedula)
        {
            _meses = meses;
            _inmuebles = inmuebles;
            _usuarios = usuarios;
            _contrato = contrato;
            _estatusc = estatusc;
            _estatuse = estatuse;
            _cuestionario = cuestionario;
            _centregable = centregable;
            _cincidencias = cincidencias;
            _cscontratos = cscontratos;
            _cedula = cedula;
            _respuestas = respuestas;
            _incidencias = incidencias;
            _entregables = entregables;
            _logs = logs;
            _ctfumigacion = ctfumigacion;
            _logse = logse;
            _pestatus = pestatus;
            _vfumigacion = vfumigacion;
            _scontratos = scontratos;
            _repositorios = repositorios;
            _facturas = facturas;
            _fcedula = fcedula;
            _veentregables = veentregables;
        }


        [Route("getCedulasByAnio/{servicio}/{anio}/{usuario}")]
        public async Task<List<CedulaFumigacionDto>> GetCedulasByAnio(int servicio, int anio, string usuario)
        {
            var inUsr = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId).ToList();
            var cedulas = (await _cedula.GetCedulaEvaluacionByAnio(anio)).Where(c => inUsr.Contains(c.InmuebleId)).ToList();
            var meses = await _meses.GetAllMesesAsync();
            var contratos = await _contrato.GetAllContratosAsync();

            cedulas = cedulas.GroupBy(c => new { c.Anio, c.MesId, c.ContratoId }).Select(c => new CedulaFumigacionDto { 
                Anio = c.Key.Anio, 
                MesId = c.Key.MesId,
                ContratoId = c.Key.ContratoId,
                Mes = meses.SingleOrDefault(m => m.Id == c.Key.MesId),
                Contrato = contratos.Single(cn => cn.Id == c.Key.ContratoId),
                TotalCedulas = c.Count()
            }).ToList();
    
            return cedulas;
        }

        
        [Route("getCedulasByAnioMes/{servicio}/{anio}/{mes}/{usuario}/{contrato}")]
        public async Task<List<CedulaEvaluacionDto>> GetCedulasByAnioMes(int servicio,int anio, int mes, int contrato, string usuario)
        {
            var inUsr = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId).ToList();

            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            var meses = await _meses.GetAllMesesAsync();
            var estatus = await _estatusc.GetAllEstatusCedulaAsync();
            var entregables = await _entregables.GetAllEntregablesAsync();
            var catalogoE = await _centregable.GetAllCTEntregables();
            var repositorios = (await _repositorios.GetAllFacturacionesAsync(anio)).SingleOrDefault(f => f.MesId == mes && f.ContratoId == contrato).Id;
            var facturas = await _facturas.GetAllFacturasAsync(repositorios);
            var respuestas = await _respuestas.GetAllRespuestasByAnioAsync(anio);

            var cedulas = (await _cedula.GetCedulaEvaluacionByAnioMes(anio, mes)).Where(c => inUsr.Contains(c.InmuebleId) && c.ContratoId == contrato) 
                          .Select(async c => new CedulaEvaluacionDto { 
                              Id = c.Id,
                              Anio = c.Anio,
                              MesId = c.MesId,
                              Mes = meses.SingleOrDefault(i => i.Id == c.MesId).Nombre,
                              InmuebleId = c.InmuebleId,
                              Inmueble = inmuebles.SingleOrDefault(i => i.Id == c.InmuebleId).Nombre,
                              Folio = c.Folio,
                              EstatusId = c.EstatusId,
                              Estatus = estatus.SingleOrDefault(i => i.Id == c.EstatusId).Nombre,
                              Fondo = estatus.SingleOrDefault(i => i.Id == c.EstatusId).Fondo,
                              RequiereNC = await _vfumigacion.VerificaDeductivas(c.Id),
                              Cedula = await _veentregables.VerificaCedulaFumigacion(c.Id),
                              Memorandum= await _veentregables.VerificaMemorandumFumigacion(c.Id),
                              ActaFirmada = await _veentregables.VerificaActaFumigacion(c.Id),
                              Factura = facturas.Where(f=>f.InmuebleId == c.InmuebleId && f.Tipo.Equals("Factura")).Sum(f => f.Total),
                              Calificacion = c.Calificacion,
                              FechaCreacion = c.FechaCreacion,
                              FechaActualizacion = c.FechaActualizacion
                          }).Select(r => r.Result).OrderBy(c=> c.Id).ToList();

            return cedulas;
        }


        [Route("getCedulasByInmuebleAM/{inmueble}/{anio}/{mes}")]
        public async Task<CedulaFumigacionDto> GetCedulaEvaluacionByInmuebleAnioMes(int inmueble, int anio, int mes)
        {
            var cedula = await _cedula.GetCedulaEvaluacionByInmuebleAnioMes(inmueble, anio, mes);

            if (cedula.Id != 0)
            {
                cedula.respuestas = await _respuestas.GetRespuestasEvaluacionByCedulaAnioMes(cedula.Id);

                return cedula;
            }
            return cedula;
        }
        

        [Route("getCedulaById/{cedula}")]
        public async Task<CedulaFumigacionDto> GetCedulaEvaluacionById(int cedula)
        {
            var cedulas = await _cedula.GetCedulaById(cedula);

            if (cedulas.Id != 0)
            {
                cedulas.Inmueble = await _inmuebles.GetInmuebleById(cedulas.InmuebleId);
                cedulas.Mes = await _meses.GetMesByIdAsync(cedulas.MesId);
                cedulas.Estatus = await _estatusc.GetECByIdAsync(cedulas.EstatusId);
                cedulas.Usuario = await _usuarios.GetUsuarioByIdAsync(cedulas.UsuarioId);
                cedulas.respuestas = await _respuestas.GetRespuestasEvaluacionByCedulaAnioMes(cedulas.Id);
                cedulas.entregables = await _entregables.GetEntregablesByCedula(cedulas.Id);
                if (await _logs.GetHistorialByCedula(cedulas.Id) != null)
                {
                    cedulas.logs = await _logs.GetHistorialByCedula(cedulas.Id);
                }

                if (await _logse.GetHistorialEntregablesByCedula(cedulas.Id) != null)
                {
                    cedulas.logs = await _logs.GetHistorialByCedula(cedulas.Id);
                }
                cedulas.logsEntregables = await _logse.GetHistorialEntregablesByCedula(cedulas.Id);

                IEnumerable<CuestionarioMensualDto> cuestionarioMensual = (await _cuestionario.GetCuestionarioMensualId(cedulas.Anio, cedulas.MesId, cedulas.ContratoId)).OrderBy(c=> c.Consecutivo);

                foreach (var dt in cedulas.respuestas)
                {
                    dt.cuestionario = await _cuestionario.GetPreguntaById(cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).CuestionarioId);
                    dt.cuestionario.Ponderacion = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Ponderacion;
                    dt.cuestionario.ACLRS = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).ACLRS;
                    dt.cuestionario.Tipo = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Tipo;
                    dt.cuestionario.Formula = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Formula;
                    dt.cuestionario.Porcentaje = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Porcentaje;
                    dt.cuestionario.Consecutivo = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Consecutivo;
                    dt.iFumigacion = await _incidencias.GetIncidenciasByPreguntaAndCedula(dt.CedulaEvaluacionId, dt.cuestionario.Consecutivo);
                    if (dt.Respuesta != null)
                    {
                        dt.ciFumigacion = await _incidencias.GetConfiguracionIncidenciasByPregunta(dt.cuestionario.Id, (bool)dt.Respuesta);
                        foreach (var cn in dt.iFumigacion)
                        {
                            cn.ciFumigacion = await _incidencias.GetConfiguracionIncidenciasByPregunta(dt.cuestionario.Id, (bool)dt.Respuesta);
                            if (cn.IncidenciaId != 0)
                            {
                                cn.Incidencia = await _cincidencias.GetIncidenciaById(cn.IncidenciaId);
                            }
                            
                            if (cn.DIncidenciaId != 0)
                            {
                                cn.DIncidencia = await _ctfumigacion.GetIncidenciaById(cn.DIncidenciaId);
                                cn.DIncidencia.Incidencia = await _cincidencias.GetIncidenciaById(cn.IncidenciaId);
                            }
                            
                                if (cn.MesId != 0)
                            {
                                cn.Mes = await _meses.GetMesByIdAsync(cn.MesId);
                            }
                        }
                    }
                }

                foreach (var en in cedulas.entregables)
                {
                    en.tipoEntregable = await _centregable.GetEntregableById(en.EntregableId);
                    if (en.UsuarioId != null)
                    {
                        en.usuario = await _usuarios.GetUsuarioByIdAsync(en.UsuarioId);
                    }
                    en.estatus = await _estatuse.GetEEByIdAsync(en.EstatusId);
                }

                foreach (var lg in cedulas.logs)
                {
                    lg.Estatus = await _estatusc.GetECByIdAsync(lg.EstatusId);
                    lg.Usuario = await _usuarios.GetUsuarioByIdAsync(lg.UsuarioId);
                }

                foreach (var lg in cedulas.logsEntregables)
                {
                    lg.Estatus = await _estatuse.GetEEByIdAsync(lg.EstatusId);
                    lg.Usuario = await _usuarios.GetUsuarioByIdAsync(lg.UsuarioId);
                    lg.Entregable = await _centregable.GetEntregableById(lg.EntregableId);
                }

                return cedulas;
            }

            return cedulas;
        }


        [Route("updateRespuestasByCedula")]
        [HttpPut]
        public async Task<IActionResult> UpdateRespuestas([FromBody] List<RespuestasUpdateCommand> respuestas)
        {
            await _respuestas.UpdateRespuestas(respuestas);
            return Ok();
        }


        [Route("updateCedula")]
        [HttpPut]
        public async Task<IActionResult> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand request)
        {
            var cedula = await _cedula.GetCedulaById(request.Id);

            if (request.Calcula)
            {
                var command = await _fcedula.EnviarCedulaEvaluacion(request, cedula);
                cedula = await _cedula.EnviarCedula(command);
            }
            else
            {
                cedula = await _cedula.UpdateCedula(request);
            }
            
            var entregables = await _pestatus.FActualizaEntregablesByEC(request);

            return Ok(cedula);
        }

        [Route("getTotalPD/{cedula}")]
        [HttpGet]
        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            return await _cedula.GetTotalPDAsync(cedula);
        }
    }
}
