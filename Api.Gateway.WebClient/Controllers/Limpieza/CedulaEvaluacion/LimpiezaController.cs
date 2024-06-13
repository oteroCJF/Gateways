using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Limpieza;
using Api.Gateway.Models.Cuestionarios.DTOs;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using Api.Gateway.Proxies.Limpieza.Contratos;
using Api.Gateway.Proxies.Limpieza.Cuestionarios;
using Api.Gateway.Proxies.Limpieza.Entregables;
using Api.Gateway.Proxies.Limpieza.Facturas;
using Api.Gateway.Proxies.Limpieza.Historiales;
using Api.Gateway.Proxies.Limpieza.Incidencias;
using Api.Gateway.Proxies.Limpieza.Repositorios;
using Api.Gateway.Proxies.Limpieza.ServicioContrato;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Estatus;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Limpieza;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.CedulaEvaluacion
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/cedulaEvaluacion")]
    public class LimpiezaController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IUsuarioProxy _usuarios;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly IEstatusEntregableProxy _estatuse;
        private readonly ILCuestionarioProxy _cuestionario;
        private readonly ICTEntregableProxy _centregable;
        private readonly ICTIncidenciaProxy _cincidencias;
        private readonly ICTILimpiezaProxy _cilimpieza;
        private readonly ICTServicioContratoProxy _cscontratos;
        private readonly ILCedulaProxy _cedula;
        private readonly ILRespuestaProxy _respuestas;
        private readonly ILIncidenciaProxy _incidencias;
        private readonly ILEntregableProxy _entregables;
        private readonly ILLogCedulaProxy _logs;
        private readonly ILLogEntregableProxy _logse;
        private readonly IEstatusProcedure _pestatus;
        private readonly ILServicioContratoProxy _scontratos;
        private readonly ILRepositorioProxy _repositorios;
        private readonly ILCFDIProxy _facturas;
        private readonly ILContratoProxy _contratos;
        private readonly IVELimpiezaProcedure _veentregables;
        private readonly IVLimpiezaProcedure _vlimpieza;

        public LimpiezaController(IMesProxy meses, IInmuebleProxy inmuebles, IUsuarioProxy usuarios, IEstatusCedulaProxy estatusc, 
                                  IEstatusEntregableProxy estatuse, ILCuestionarioProxy cuestionario, ICTEntregableProxy centregable, 
                                  ICTIncidenciaProxy cincidencias, ICTServicioContratoProxy cscontratos, ILCedulaProxy cedula, 
                                  ILRespuestaProxy respuestas, ILIncidenciaProxy incidencias, ILEntregableProxy entregables,
                                  ILLogCedulaProxy logs, ILLogEntregableProxy logse, IEstatusProcedure pestatus, 
                                  ILServicioContratoProxy scontratos, ILRepositorioProxy repositorios, ILCFDIProxy facturas,
                                  ICTILimpiezaProxy cilimpieza, ILContratoProxy contratos, IVELimpiezaProcedure veentregables,
                                  IVLimpiezaProcedure vlimpieza)
        {
            _meses = meses;
            _inmuebles = inmuebles;
            _usuarios = usuarios;
            _contratos = contratos;
            _estatusc = estatusc;
            _estatuse = estatuse;
            _cuestionario = cuestionario;
            _centregable = centregable;
            _cincidencias = cincidencias;
            _cscontratos = cscontratos;
            _cedula = cedula;
            _respuestas = respuestas;
            _incidencias = incidencias;
            _cilimpieza = cilimpieza;
            _entregables = entregables;
            _logs = logs;
            _logse = logse;
            _pestatus = pestatus;
            _scontratos = scontratos;
            _repositorios = repositorios;
            _facturas = facturas;
            _veentregables = veentregables;
            _vlimpieza = vlimpieza;
        }


        [Route("getCedulasByAnio/{servicio}/{anio}/{usuario}")]
        public async Task<List<CedulaLimpiezaDto>> GetCedulasByAnio(int servicio, int anio, string usuario)
        {
            var inUsr = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId).ToList();
            var cedulas = (await _cedula.GetCedulaEvaluacionByAnio(anio)).Where(c => inUsr.Contains(c.InmuebleId)).ToList();
            var meses = await _meses.GetAllMesesAsync();
            var contratos = await _contratos.GetAllContratosAsync();

            cedulas = cedulas.GroupBy(c => new { c.Anio, c.MesId, c.ContratoId }).Select(c => new CedulaLimpiezaDto
            {
                Anio = c.Key.Anio,
                MesId = c.Key.MesId,
                ContratoId = c.Key.ContratoId,
                Mes = meses.SingleOrDefault(m => m.Id == c.Key.MesId),
                Contrato = contratos.Single(cn =>cn.Id == c.Key.ContratoId),
                TotalCedulas = c.Count()
            }).ToList();

            return cedulas;
        }


        [Route("getCedulasByAnioMes/{servicio}/{anio}/{mes}/{usuario}/{contrato}")]
        public async Task<List<CedulaEvaluacionDto>> GetCedulasByAnioMes(int servicio, int anio, int mes, int contrato, string usuario)
        {
            var inUsr = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId).ToList();

            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            var meses = await _meses.GetAllMesesAsync();
            var estatus = await _estatusc.GetAllEstatusCedulaAsync();
            var entregables = await _entregables.GetAllEntregablesAsync();
            var catalogoE = await _centregable.GetAllCTEntregables();
            var repositorios = (await _repositorios.GetAllRepositoriosAsync(anio)).SingleOrDefault(f => f.MesId == mes && f.ContratoId == contrato).Id;
            var facturas = await _facturas.GetAllFacturasAsync(repositorios);
            var respuestas = await _respuestas.GetAllRespuestasByAnioAsync(anio);

            var cedulas = (await _cedula.GetCedulaEvaluacionByAnioMes(anio, mes)).Where(c => inUsr.Contains(c.InmuebleId) && c.ContratoId == contrato)
                          .Select(async c => new CedulaEvaluacionDto
                          {
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
                              RequiereNC = await _vlimpieza.VerificaDeductivas(c.Id),
                              Cedula = await _veentregables.VerificaCedulaLimpieza(c.Id),
                              Memorandum= await _veentregables.VerificaMemorandumLimpieza(c.Id),
                              ActaFirmada = await _veentregables.VerificaActaLimpieza(c.Id),
                              Factura = facturas.Where(f => f.InmuebleId == c.InmuebleId && f.Tipo.Equals("Factura")).Sum(f => f.Total),
                              Calificacion = c.Calificacion,
                              FechaCreacion = c.FechaCreacion,
                              FechaActualizacion = c.FechaActualizacion
                          })
                          .Select(r => r.Result).OrderBy(c => c.Id).ToList();

            return cedulas;
        }


        [Route("getCedulasByInmuebleAM/{inmueble}/{anio}/{mes}")]
        public async Task<CedulaLimpiezaDto> GetCedulaEvaluacionByInmuebleAnioMes(int inmueble, int anio, int mes)
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
        public async Task<CedulaLimpiezaDto> GetCedulaEvaluacionById(int cedula)
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

                IEnumerable<CuestionarioMensualDto> cuestionarioMensual = await _cuestionario.GetCuestionarioMensualId(cedulas.Anio, cedulas.MesId, cedulas.ContratoId);

                foreach (var dt in cedulas.respuestas)
                {
                    dt.cuestionario = await _cuestionario.GetPreguntaById(cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).CuestionarioId);
                    dt.cuestionario.Ponderacion = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Ponderacion;
                    dt.cuestionario.ACLRS = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).ACLRS;
                    dt.cuestionario.Tipo = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Tipo;
                    dt.cuestionario.Formula = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Formula;
                    dt.cuestionario.Porcentaje = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Porcentaje;
                    dt.cuestionario.Consecutivo = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Consecutivo;
                    dt.iLimpieza = await _incidencias.GetIncidenciasByPreguntaAndCedula(dt.CedulaEvaluacionId, dt.cuestionario.Consecutivo);
                    if (dt.Respuesta != null)
                    {
                        dt.ciLimpieza = await _incidencias.GetConfiguracionIncidenciasByPregunta(dt.cuestionario.Id, (bool)dt.Respuesta);
                        foreach (var cn in dt.iLimpieza)
                        {
                            cn.ciLimpieza = await _incidencias.GetConfiguracionIncidenciasByPregunta(dt.cuestionario.Id, (bool)dt.Respuesta);
                            if (cn.IncidenciaId != 0)
                            {
                                cn.Incidencia = await _cilimpieza.GetIncidenciaById(cn.IncidenciaId);
                                cn.Incidencia.Incidencia = await _cincidencias.GetIncidenciaById(cn.Incidencia.IncidenciaId);
                            }

                            if (cn.MesId != 0)
                            {
                                cn.Mes = await _meses.GetMesByIdAsync((int)cn.MesId);
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

            cedula = await _cedula.UpdateCedula(request);
            cedula.Estatus = await _estatusc.GetECByIdAsync(cedula.EstatusId);

            if (cedula != null)
            {
                var estatusE = await _pestatus.EnviarCedula(cedula.EstatusId);
                List<EntregableDto> entregable = await _entregables.GetEntregablesByCedula(cedula.Id);
                EEntregableUpdateCommand update = null;

                foreach (var en in entregable)
                {
                    en.estatus = await _estatuse.GetEEByIdAsync(en.EstatusId);

                    if (en.estatus.Nombre.Equals("En Proceso"))
                    {
                        update = new EEntregableUpdateCommand
                        {
                            Id = en.Id,
                            UsuarioId = en.UsuarioId,
                            EstatusId = estatusE,
                            Observaciones = "Se envía el entregable para su revisión."
                        };

                        await _entregables.AUpdateEntregable(update);
                    }
                    else if (en.estatus.Nombre.Equals("En Revisión"))
                    {
                        update = new EEntregableUpdateCommand
                        {
                            Id = en.Id,
                            UsuarioId = en.UsuarioId,
                            EstatusId = estatusE,
                            Observaciones = "Se cambia el entregable de estatus."
                        };

                        await _entregables.AUpdateEntregable(update);
                    }
                }
            }

            cedula.Estatus = await _estatusc.GetECByIdAsync(cedula.EstatusId);

            return Ok(cedula);
        }


        [Route("rechazarCedula")]
        [HttpPut]
        public async Task<IActionResult> RechazarCedula([FromBody] CedulaEvaluacionUpdateCommand request)
        {

            var cedula = await _cedula.UpdateCedula(request);
            cedula.Estatus = await _estatusc.GetECByIdAsync(cedula.EstatusId);

            if (cedula != null)
            {
                List<EntregableDto> entregable = await _entregables.GetEntregablesByCedula(cedula.Id);
                ESREntregableUpdateCommand update = null;

                foreach (var en in entregable)
                {
                    en.estatus = await _estatuse.GetEEByIdAsync(en.EstatusId);
                    en.tipoEntregable = await _centregable.GetEntregableById(en.EntregableId);

                    if (!en.tipoEntregable.Abreviacion.Equals("Factura") && !en.tipoEntregable.Abreviacion.Equals("SAT") &&
                        !en.tipoEntregable.Abreviacion.Equals("Reporte(s)") && !en.tipoEntregable.Abreviacion.Equals("ActaInicio"))
                    {
                        update = new ESREntregableUpdateCommand
                        {
                            Id = en.Id,
                            UsuarioId = en.UsuarioId,
                            EstatusId = en.EstatusId,
                            Aprobada = true,
                            Observaciones = "Se elimina el entregable ya que se rechazó la cédula."
                        };
                    }
                    else
                    {
                        update = new ESREntregableUpdateCommand
                        {
                            Id = en.Id,
                            UsuarioId = en.UsuarioId,
                            EstatusId = await _pestatus.CedulaSolicitudRechazo(en.EstatusId),
                            Aprobada = false,
                            Observaciones = "Se regresa a \"En Proceso\" el entregable ya que se rechazó la cédula."
                        };
                    }

                    await _entregables.UpdateEntregableSR(update);
                }
            }

            cedula.Estatus = await _estatusc.GetECByIdAsync(cedula.EstatusId);

            return Ok(cedula);
        }


        [Route("cedulaSolicitudRechazo")]
        [HttpPut]
        public async Task<IActionResult> CedulaSolicitudRechazo([FromBody] CedulaSRUpdateCommand request)
        {
            var cedula = await _cedula.CedulaSolicitudRechazo(request);

            return Ok(cedula);
        }


        [Route("denegarSolicitudRechazo")]
        [HttpPut]
        public async Task<IActionResult> DenegarSolicitudRechazo([FromBody] CedulaSRUpdateCommand request)
        {
            var cedula = await _cedula.CedulaSolicitudRechazo(request);
            if (cedula != null)
            {
                List<EntregableDto> entregable = await _entregables.GetEntregablesByCedula(cedula.Id);
                ESREntregableUpdateCommand update = null;

                foreach (var en in entregable)
                {
                    en.estatus = await _estatuse.GetEEByIdAsync(en.EstatusId);
                    en.tipoEntregable = await _centregable.GetEntregableById(en.EntregableId);

                    if (!en.tipoEntregable.Abreviacion.Equals("Factura") && !en.tipoEntregable.Abreviacion.Equals("SAT") &&
                        !en.tipoEntregable.Abreviacion.Equals("Reporte(s)") && !en.tipoEntregable.Abreviacion.Equals("ActaInicio"))
                    {
                        update = new ESREntregableUpdateCommand
                        {
                            Id = en.Id,
                            UsuarioId = en.UsuarioId,
                            EstatusId = en.EstatusId,
                            Aprobada = false,
                            Observaciones = "La solicitud de rechazó se denegó."
                        };

                        await _entregables.UpdateEntregableSR(update);
                    }
                }
            }
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
