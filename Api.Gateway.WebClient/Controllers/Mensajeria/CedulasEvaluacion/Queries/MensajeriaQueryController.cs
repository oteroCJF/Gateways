﻿using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Mensajeria;
using Api.Gateway.Models.Cuestionarios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion;
using Api.Gateway.Proxies.Mensajeria.Entregables;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Gateway.Proxies.Catalogos.CTIndemnizacion;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Estatus;
using Api.Gateway.Proxies.Mensajeria.Respuestas.Queries;
using Api.Gateway.Models;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies.Mensajeria.Cuestionarios.Queries;
using Api.Gateway.Proxies.Mensajeria.LogCedulas.Queries;
using Api.Gateway.Proxies.Mensajeria.LogEntregables.Queries;
using Api.Gateway.Proxies.Mensajeria.Repositorios.Queries;
using Api.Gateway.Proxies.Mensajeria.CFDIs.Queries;
using Api.Gateway.Proxies.Mensajeria.Contratos.Queries;
using Api.Gateway.WebClient.Controllers.Mensajeria.CedulasEvaluacion.Procedures;
using Api.Gateway.Proxies.Mensajeria.Incidencias.Queries;
using Api.Gateway.Proxies.Mensajeria.Incidencias.Commands;
using Api.Gateway.Proxies.Mensajeria.SoportePago.Queries;
using Api.Gateway.Proxies.Mensajeria.SoportePago.Commands;
using Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Procedures.Queries;
using System.Diagnostics.Contracts;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.CedulasEvaluacion.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/cedulaEvaluacion")]
    public class MensajeriaQueryController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IUsuarioProxy _usuarios;

        private readonly IEstatusCedulaProxy _estatusCedula;
        private readonly IEstatusEntregableProxy _estatuse;
        
        private readonly ICTEntregableProxy _ctEntregables;
        private readonly ICTIncidenciaProxy _ctIncidencias;
        private readonly ICTIndemnizacionProxy _indemnizacion;

        private readonly IQCedulaMensajeriaProxy _cedula;
        private readonly IQRespuestaMensajeriaProxy _respuestas;
        private readonly IQIncidenciaMensajeriaProxy _incidencias;
        private readonly IQCuestionarioMensajeriaProxy _cuestionario;

        private readonly IQEntregableMensajeriaProxy _entregables;
        private readonly IQLCedulaMensajeriaProxy _logs;
        private readonly IQLEntregableMensajeriaProxy _logse;
        private readonly IQRepositorioMensajeriaProxy _repositorios;
        private readonly IQCFDIMensajeriaProxy _facturas;
        private readonly IQSoportePagoMensajeriaProxy _soporte;
        private readonly IQContratoMensajeriaProxy _contratos;

        private readonly IEstatusProcedure _pestatus;
        private readonly ICedulaMensajeriaProcedure _pcedula;
        private readonly IQMensajeriaEntregableProcedure _entregableProcedure;

        public MensajeriaQueryController(IMesProxy meses, IInmuebleProxy inmuebles, IUsuarioProxy usuarios, IEstatusCedulaProxy estatusCedula, 
                                         IEstatusEntregableProxy estatuse, ICTEntregableProxy ctEntregables, ICTIncidenciaProxy ctIncidencias, 
                                         ICTIndemnizacionProxy indemnizacion, IQCedulaMensajeriaProxy cedula, IQRespuestaMensajeriaProxy respuestas,
                                         IQIncidenciaMensajeriaProxy incidencias, IQCuestionarioMensajeriaProxy cuestionario, IQEntregableMensajeriaProxy entregables, 
                                         IQLCedulaMensajeriaProxy logs, IQLEntregableMensajeriaProxy logse, IQRepositorioMensajeriaProxy repositorios, 
                                         IQCFDIMensajeriaProxy facturas, IQSoportePagoMensajeriaProxy soporte, IQContratoMensajeriaProxy contratos, 
                                         IEstatusProcedure pestatus, ICedulaMensajeriaProcedure pcedula, IQMensajeriaEntregableProcedure entregableProcedure)
        {
            _meses = meses;
            _inmuebles = inmuebles;
            _usuarios = usuarios;
            _estatusCedula = estatusCedula;
            _estatuse = estatuse;
            _ctEntregables = ctEntregables;
            _ctIncidencias = ctIncidencias;
            _indemnizacion = indemnizacion;
            _cedula = cedula;
            _respuestas = respuestas;
            _incidencias = incidencias;
            _cuestionario = cuestionario;
            _entregables = entregables;
            _logs = logs;
            _logse = logse;
            _repositorios = repositorios;
            _facturas = facturas;
            _soporte = soporte;
            _contratos = contratos;
            _pestatus = pestatus;
            _pcedula = pcedula;
            _entregableProcedure = entregableProcedure;
        }


        [Route("getCedulasByAnio/{servicio}/{anio}/{usuario}")]
        [HttpGet]
        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulasByAnio(int servicio, int anio, string usuario)
        {
            var inUsr = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId).ToList();

            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            var meses = await _meses.GetAllMesesAsync();
            var estatus = await _estatusCedula.GetAllEstatusCedulaAsync();
            var entregables = await _entregables.GetEntregablesValidados();
            var catalogoE = await _ctEntregables.GetAllCTEntregables();
            var repositorios = await _repositorios.GetAllRepositoriosAsync(anio);

            var repositoriosId = (await _repositorios.GetAllRepositoriosAsync(anio)).Select(r => r.Id);
            var facturas = (await _facturas.GetAllFacturas()).Where(f => repositoriosId.Contains(f.RepositorioId) && f.Tipo.Equals("Factura"));

            var result = await _cedula.GetCedulaEvaluacionByAnio(anio);

            var cedulas = result.Items.Where(c => inUsr.Contains(c.InmuebleId)).ToList();

            result.Items = cedulas;

            if (result.Items != null)
            {
                foreach (var ced in result.Items)
                {
                    try
                    {
                        var idActa = catalogoE.Single(ct => ct.Abreviacion.Equals("ActaER")).Id;
                        var idMemorandum = catalogoE.Single(ct => ct.Abreviacion.Equals("Memorandum")).Id;
                        var repositorioId = repositorios.Single(r => r.MesId == ced.MesId && r.Anio == ced.Anio && r.ContratoId == ced.ContratoId).Id;

                        ced.Mes = meses.Single(m => m.Id == ced.MesId).Nombre;
                        ced.Inmueble = inmuebles.Single(i => i.Id == ced.InmuebleId).Nombre;
                        ced.Estatus = estatus.Single(e => e.Id == ced.EstatusId).Nombre;
                        ced.Fondo = estatus.Single(e => e.Id == ced.EstatusId).Fondo;
                        ced.Factura = facturas.Where(f => f.InmuebleId == ced.InmuebleId && f.RepositorioId == repositorioId).Sum(f => f.Total);
                        ced.Cedula = _entregableProcedure.VerificaCedulaMensajeria(entregables, catalogoE, ced.Id);
                        ced.ActaFirmada = _entregableProcedure.VerificaActaMensajeria(entregables, catalogoE, ced.Id);
                        ced.Memorandum = _entregableProcedure.VerificaMemorandumMensajeria(entregables, catalogoE, ced.Id);
                    }
                    catch(System.Exception EX)
                    {
                        string msg = EX.Message;
                        return null;
                    }
                }
            }

            return result;
        }
        
        [Route("getCedulasByAnioMes/{servicio}/{anio}/{mes}/{contrato}/{usuario}")]
        [HttpGet]
        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulasByAnioMes(int servicio, int anio, int mes, int contrato, string usuario)
        {
            var inUsr = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId).ToList();

            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            var meses = await _meses.GetAllMesesAsync();
            var estatus = await _estatusCedula.GetAllEstatusCedulaAsync();
            
            var repositorio = (await _repositorios.GetAllRepositoriosAsync(anio)).Select(r => r.Id);
            var facturas = (await _facturas.GetAllFacturas()).Where(f => repositorio.Contains(f.RepositorioId) && f.Tipo.Equals("Factura"));

            var result = await _cedula.GetCedulaEvaluacionByAnioMes(anio, mes, contrato);

            var cedulas = result.Items.Where(c => inUsr.Contains(c.InmuebleId) && c.ContratoId == contrato).ToList();

            result.Items = cedulas;

            if (result.Items != null)
            {
                foreach (var ced in result.Items)
                {
                    ced.Mes = meses.Single(m => m.Id == ced.MesId).Nombre;
                    ced.Inmueble = inmuebles.Single(i => i.Id == ced.InmuebleId).Nombre;
                    ced.Estatus = estatus.Single(e => e.Id == ced.EstatusId).Nombre;
                    ced.Fondo = estatus.Single(e => e.Id == ced.EstatusId).Fondo;
                    ced.Factura = facturas.Where(f => f.InmuebleId == ced.InmuebleId).Sum(f => f.Total);
                }
            }

            return result;
        }


        [Route("getCedulaById/{id}")]
        public async Task<CedulaMensajeriaDto> GetCedulaEvaluacionById(int id)
        {

            var ctEstatusCedula = await _estatusCedula.GetAllEstatusCedulaAsync();
            var ctEstatusEntregables = await _estatuse.GetAllEstatusEntregablesAsync();
            var configuracionIncidencias = await _incidencias.GetConfiguracionIncidencias();
            var ctEntregables = await _ctEntregables.GetAllCTEntregables();

            var cedula = await _cedula.GetCedulaById(id);

            if (cedula.Id != 0)
            {
                cedula.Inmueble = await _inmuebles.GetInmuebleById(cedula.InmuebleId);
                cedula.Contrato = await _contratos.GetContratoByIdAsync(cedula.ContratoId);
                cedula.Mes = await _meses.GetMesByIdAsync(cedula.MesId);
                cedula.Estatus = ctEstatusCedula.Single(ec => ec.Id == cedula.EstatusId);
                cedula.Usuario = await _usuarios.GetUsuarioByIdAsync(cedula.UsuarioId);
                cedula.respuestas = await _respuestas.GetRespuestasEvaluacionByCedulaAnioMes(id);
                cedula.entregables = await _entregables.GetEntregablesByCedula(id);

                if (await _logs.GetHistorialByCedula(cedula.Id) != null)
                {
                    cedula.logs = await _logs.GetHistorialByCedula(cedula.Id);
                }

                if (await _logse.GetHistorialEntregablesByCedula(cedula.Id) != null)
                {
                    cedula.logsEntregables = await _logse.GetHistorialEntregablesByCedula(cedula.Id);
                }

                List<CuestionarioMensualDto> cuestionarioMensual = await _cuestionario.GetCuestionarioMensualId(cedula.Anio, cedula.MesId, cedula.ContratoId);

                foreach (var dt in cedula.respuestas)
                {
                    dt.cuestionario = await _cuestionario.GetPreguntaById(cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).CuestionarioId);
                    dt.cuestionario.Ponderacion = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Ponderacion;
                    dt.cuestionario.ACLRS = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).ACLRS;
                    dt.cuestionario.Tipo = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Tipo;
                    dt.cuestionario.Formula = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Formula;
                    dt.cuestionario.Porcentaje = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Porcentaje;
                    dt.cuestionario.Consecutivo = cuestionarioMensual.SingleOrDefault(p => dt.Pregunta == p.Consecutivo).Consecutivo;
                    dt.iMensajeria = await _incidencias.GetIncidenciasByPreguntaAndCedula(dt.CedulaEvaluacionId, dt.cuestionario.Consecutivo);
                    if (dt.Respuesta != null)
                    {
                        dt.ciMensajeria = configuracionIncidencias.Single(ci => ci.Pregunta == dt.cuestionario.Id && ci.Respuesta == (bool)dt.Respuesta);
                        foreach (var cn in dt.iMensajeria)
                        {
                            cn.ciMensajeria = configuracionIncidencias.Single(ci => ci.Pregunta == dt.cuestionario.Id && ci.Respuesta == (bool)dt.Respuesta);
                            if (cn.IncidenciaId != 0)
                            {
                                cn.Incidencia = await _ctIncidencias.GetIncidenciaById(cn.IncidenciaId);
                            }

                            if (cn.IndemnizacionId != 0)
                            {
                                cn.Indemnizacion = await _indemnizacion.GetIndemnizacionById(cn.IndemnizacionId);
                            }
                        }
                    }
                }

                foreach (var en in cedula.entregables)
                {
                    en.tipoEntregable = ctEntregables.Single(cte => cte.Id == en.EntregableId);
                    if (en.UsuarioId != null)
                    {
                        en.usuario = await _usuarios.GetUsuarioByIdAsync(en.UsuarioId);
                    }
                    en.estatus = ctEstatusEntregables.Single(ctee => ctee.Id == en.EstatusId);
                }

                foreach (var lg in cedula.logs)
                {
                    lg.Estatus = ctEstatusCedula.Single(ctec => ctec.Id == lg.EstatusId);
                    lg.Usuario = await _usuarios.GetUsuarioByIdAsync(lg.UsuarioId);
                }

                foreach (var lg in cedula.logsEntregables)
                {
                    lg.Estatus = ctEstatusEntregables.Single(ctee => ctee.Id == lg.EstatusId);
                    lg.Usuario = await _usuarios.GetUsuarioByIdAsync(lg.UsuarioId);
                    lg.Entregable = ctEntregables.Single(cte=> cte.Id == lg.EntregableId);
                }

                return cedula;
            }

            return cedula;
        }


        [Route("getTotalPD/{cedula}")]
        [HttpGet]
        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            return await _cedula.GetTotalPDAsync(cedula);
        }

        [Route("getReportePAT/{anio}/{mes}")]
        public async Task<DataCollection<CedulaEvaluacionDto>> GetReportePAT(int anio, int mes)
        {
            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            var meses = await _meses.GetAllMesesAsync();
            var contratos = await _contratos.GetAllContratosAsync();



            var result = await _cedula.GetCedulaEvaluacionByAnio(anio);

            var cedulas = result.Items.Where(c => c.MesId == mes).ToList();

            result.Items = cedulas;

            if (result.Items != null)
            {
                foreach (var ced in result.Items)
                {
                    ced.Mes = meses.Single(m => m.Id == ced.MesId).Nombre;
                    ced.Inmueble = inmuebles.Single(i => i.Id == ced.InmuebleId).Nombre;
                }
            }

            return result;
        }
    }
}
