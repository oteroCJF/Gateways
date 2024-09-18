using Api.Gateway.Models;
using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Comedor;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Cuestionarios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Queries;
using Api.Gateway.Proxies.Comedor.CFDIs.Queries;
using Api.Gateway.Proxies.Comedor.Contratos.Queries;
using Api.Gateway.Proxies.Comedor.Cuestionarios.Queries;
using Api.Gateway.Proxies.Comedor.Entregables.Queries;
using Api.Gateway.Proxies.Comedor.LogCedula.Queries;
using Api.Gateway.Proxies.Comedor.LogEntregables.Queries;
using Api.Gateway.Proxies.Comedor.Repositorios.Queries;
using Api.Gateway.Proxies.Comedor.Respuestas.Queries;
using Api.Gateway.Proxies.Comedor.ServiciosContrato.Queries;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Api.Gateway.WebClient.Controllers.Comedor.CedulasEvaluacion.Procedures;
using Api.Gateway.WebClient.Controllers.Comedor.Entregables.Procedures;
using Api.Gateway.WebClient.Controllers.Comedor.Entregables.Procedures.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.CedulasEvaluacion.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/cedulaEvaluacion")]
    public class ComedorController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IUsuarioProxy _usuarios;
        private readonly ICTParametroProxy _parametros;

        private readonly IEstatusCedulaProxy _estatusCedula;
        private readonly IEstatusEntregableProxy _estatusEntregables;

        private readonly IQCuestionarioComedorProxy _cuestionarios;
        private readonly ICTEntregableProxy _ctEntregables;
        private readonly ICTIncidenciaProxy _ctIncidencias;
        private readonly ICTServicioContratoProxy _ctSContratos;
        private readonly IQCedulaComedorProxy _cedula;
        private readonly ICCedulaComedorProxy _cedulaCommand;
        private readonly IQRespuestaComedorProxy _respuestas;
        private readonly IQEntregableComedorProxy _entregables;
        private readonly IQLCedulaComedorProxy _logsCedula;
        private readonly IQLEntregableComedorProxy _logsEntregables;
        private readonly IQSContratoComedorProxy _serviciosContratos;
        private readonly IQRepositorioComedorProxy _repositorios;
        private readonly IQCFDIComedorProxy _facturas;
        private readonly IQContratoComedorProxy _contrato;
        private readonly IQComedorEntregableProcedure _entregableProcedure;
        private readonly ICedulaComedorProcedure _cedulaProcedure;
        private readonly ICuestionarioComedorProcedure _respuestasProcedure;

        public ComedorController(IMesProxy meses, IInmuebleProxy inmuebles, IUsuarioProxy usuarios, IEstatusCedulaProxy estatusCedula, 
                                 IEstatusEntregableProxy estatusEntregables, IQCuestionarioComedorProxy cuestionarios, ICTEntregableProxy ctEntregables, 
                                 ICTIncidenciaProxy ctIncidencias, ICTServicioContratoProxy ctSContratos, IQCedulaComedorProxy cedula, 
                                 IQEntregableComedorProxy entregables, IQLCedulaComedorProxy logsCedula, IQLEntregableComedorProxy logsEntregables, 
                                 IQSContratoComedorProxy serviciosContratos, IQRepositorioComedorProxy repositorios, IQCFDIComedorProxy facturas, 
                                 IQContratoComedorProxy contrato, IQComedorEntregableProcedure entregableProcedure, ICCedulaComedorProxy cedulaCommand,
                                 ICedulaComedorProcedure cedulaProcedure, IQRespuestaComedorProxy respuestas, ICTParametroProxy parametros, 
                                 ICuestionarioComedorProcedure respuestasProcedure)
        {
            _meses = meses;
            _parametros = parametros;
            _inmuebles = inmuebles;
            _usuarios = usuarios;
            _estatusCedula = estatusCedula;
            _estatusEntregables = estatusEntregables;
            _cuestionarios = cuestionarios;
            _ctEntregables = ctEntregables;
            _ctIncidencias = ctIncidencias;
            _ctSContratos = ctSContratos;
            _cedulaCommand = cedulaCommand;
            _cedula = cedula;
            _cedulaProcedure = cedulaProcedure;
            _entregables = entregables;
            _respuestas = respuestas;
            _logsCedula = logsCedula;
            _logsEntregables = logsEntregables;
            _serviciosContratos = serviciosContratos;
            _repositorios = repositorios;
            _facturas = facturas;
            _contrato = contrato;
            _entregableProcedure = entregableProcedure;
            _respuestasProcedure = respuestasProcedure;
        }


        [Route("getCedulasByAnio/{servicio}/{anio}/{usuario}")]
        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulasByAnio(int servicio, int anio, string usuario)
        {
            var inUsr = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId).ToList();

            var facturas = new List<CFDIDto>();

            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            var meses = await _meses.GetAllMesesAsync();
            var estatus = await _estatusCedula.GetAllEstatusCedulaAsync();
            var entregables = await _entregables.GetEntregablesValidados();
            var catalogoE = await _ctEntregables.GetAllCTEntregables();
            var catalogoS = await _ctSContratos.GetAllServiciosContratosAsync();
            var repositoriosId = (await _repositorios.GetAllRepositoriosAsync(anio)).Select(f => f.Id).ToList();
            var repositorios = await _repositorios.GetAllRepositoriosAsync(anio);
            var respuestas = await _respuestas.GetAllRespuestasByAnioAsync(anio);

            if (anio > 2023) {
                facturas = (await _facturas.GetAllFacturas()).Where(f => repositoriosId.Contains(f.RepositorioId)).ToList();
            }
            
            var result = await _cedula.GetCedulaEvaluacionByAnio(anio);

            if (result.Items != null)
            {
                foreach (var ced in result.Items)
                {
                    var idActa = catalogoE.Single(ct => ct.Abreviacion.Equals("ActaER")).Id;
                    var idMemorandum = catalogoE.Single(ct => ct.Abreviacion.Equals("Memorandum")).Id;
                    var preguntasDeductiva = await _cuestionarios.GetPreguntasDeductiva(ced.Anio, ced.MesId, ced.ContratoId, ced.ServicioId);
                    var repositorioId = repositorios.Single(r => r.Anio == ced.Anio && r.MesId == ced.MesId
                                            && r.ContratoId == ced.ContratoId);

                    ced.Mes = meses.Single(m => m.Id == ced.MesId).Nombre;
                    ced.Inmueble = inmuebles.Single(i => i.Id == ced.InmuebleId).Nombre;
                    ced.Estatus = estatus.Single(e => e.Id == ced.EstatusId).Nombre;
                    ced.Fondo = estatus.Single(e => e.Id == ced.EstatusId).Fondo;
                    ced.Servicio = catalogoS.Single(sc => sc.Id == ced.ServicioId).Nombre;
                    if (ced.Anio>2023)
                    {
                        ced.Factura = facturas.Where(f => f.InmuebleId == ced.InmuebleId && f.Tipo.Equals("Factura") &&
                                                        f.RepositorioId == repositorioId.Id).Sum(f => f.Total);
                        ced.NC = facturas.Where(f => f.InmuebleId == ced.InmuebleId && f.Tipo.Equals("NC") &&
                                                        f.RepositorioId == repositorioId.Id).Sum(f => f.Total);
                    }
                    ced.Cedula = _entregableProcedure.VerificaCedulaComedor(entregables, catalogoE, ced.Id);
                    ced.ActaFirmada = _entregableProcedure.VerificaActaComedor(entregables, catalogoE, ced.Id);
                    ced.Memorandum = _entregableProcedure.VerificaMemorandumComedor(entregables, catalogoE, ced.Id);
                    ced.TotalDeductivas = respuestas.Where(r => r.CedulaEvaluacionId == ced.Id && preguntasDeductiva.Contains(r.Pregunta)).Sum(r => r.MontoPenalizacion);
                }
            }

            return result;
        }


        [Route("getCedulaById/{cedula}")]
        public async Task<CedulaComedorDto> GetCedulaEvaluacionById(int cedula)
        {
            var cedulas = await _cedula.GetCedulaById(cedula);

            if (cedulas.Id != 0)
            {
                cedulas.Inmueble = await _inmuebles.GetInmuebleById(cedulas.InmuebleId);
                cedulas.Mes = await _meses.GetMesByIdAsync(cedulas.MesId);
                cedulas.Estatus = await _estatusCedula.GetECByIdAsync(cedulas.EstatusId);
                cedulas.Contrato = await _contrato.GetContratoByIdAsync(cedulas.ContratoId);
                cedulas.Usuario = await _usuarios.GetUsuarioByIdAsync(cedulas.UsuarioId);
                cedulas.Servicio = await _ctSContratos.GetServicioContratoByIdAsync(cedulas.ServicioId);
                cedulas.entregables = await _entregables.GetEntregablesByCedula(cedulas.Id);
                cedulas.Cuestionario = await _respuestasProcedure.GetCuestionarioComedor(cedulas);


                if (await _logsCedula.GetHistorialByCedula(cedulas.Id) != null)
                {
                    cedulas.logs = await _logsCedula.GetHistorialByCedula(cedulas.Id);
                }
                
                if (await _logsEntregables.GetHistorialEntregablesByCedula(cedulas.Id) != null)
                {
                    cedulas.logsEntregables = await _logsEntregables.GetHistorialEntregablesByCedula(cedulas.Id);
                }
               
                foreach (var en in cedulas.entregables)
                {
                    en.tipoEntregable = await _ctEntregables.GetEntregableById(en.EntregableId);
                    if (en.UsuarioId != null)
                    {
                        en.usuario = await _usuarios.GetUsuarioByIdAsync(en.UsuarioId);
                    }
                    en.estatus = await _estatusEntregables.GetEEByIdAsync(en.EstatusId);
                }

                foreach (var lg in cedulas.logs)
                {
                    lg.Estatus = await _estatusCedula.GetECByIdAsync(lg.EstatusId);
                    lg.Usuario = await _usuarios.GetUsuarioByIdAsync(lg.UsuarioId);
                }

                foreach (var lg in cedulas.logsEntregables)
                {
                    lg.Estatus = await _estatusEntregables.GetEEByIdAsync(lg.EstatusId);
                    lg.Usuario = await _usuarios.GetUsuarioByIdAsync(lg.UsuarioId);
                    lg.Entregable = await _ctEntregables.GetEntregableById(lg.EntregableId);
                }

                return cedulas;
            }

            return cedulas;
        }


        [Route("getTotalPD/{cedula}")]
        [HttpGet]
        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            return await _cedula.GetTotalPDAsync(cedula);
        }
    }
}
