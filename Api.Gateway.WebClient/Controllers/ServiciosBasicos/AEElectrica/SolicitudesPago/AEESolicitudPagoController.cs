using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Entregables.ServiciosBasicos.Commands;
using Api.Gateway.Models.Entregables.ServiciosBasicos.DTOs;
using Api.Gateway.Models.SolicitudesPago.Commands;
using Api.Gateway.Models.SolicitudesPago.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.CFDIs;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.Entregables;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.LogEntregables;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.LogSolicitudes;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.SolicitudesPagos;
using Api.Gateway.Proxies.Usuarios;
using Api.Gateway.WebClient.Procedures.ServiciosBasicos.AEElectrica;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.ServiciosBasicos.AEElectrica.SolicitudesPago
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("aeelectrica/solicitudes")]
    public class AEESolicitudPagoController :ControllerBase
    {
        private readonly IAEESolicitudPagoProxy _solicitud;
        private readonly IAEEEntregableProxy _entregables;
        private readonly ICTEntregableProxy _ctentregables;
        private readonly ICTServicioProxy _ctservicios;
        private readonly IAEECFDIProxy _cfdi;
        private readonly IEstatusSPProxy _estatussp;
        private readonly IEstatusEntregableProxy _estatuse;
        private readonly IUsuarioProxy _usuarios;
        private readonly IMesProxy _mes;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IAEELogSolicitudProxy _lsolicitud;
        private readonly IAEELogEntregableProxy _lentregable;

        private readonly IAEElectricaProcedure _aeeProcedure;

        public AEESolicitudPagoController(IAEESolicitudPagoProxy solicitud, IAEEEntregableProxy entregables, ICTEntregableProxy ctentregables, 
                                          ICTServicioProxy ctservicios, IAEECFDIProxy cfdi, IEstatusSPProxy estatussp, 
                                          IEstatusEntregableProxy estatuse, IUsuarioProxy usuarios, IMesProxy mes, IInmuebleProxy inmuebles, 
                                          IAEELogSolicitudProxy lsolicitud, IAEELogEntregableProxy lentregable,
                                          IAEElectricaProcedure aeeProcedure)
        {
            _solicitud = solicitud;
            _entregables = entregables;
            _ctentregables = ctentregables;
            _ctservicios = ctservicios;
            _cfdi = cfdi;
            _estatussp = estatussp;
            _estatuse = estatuse;
            _usuarios = usuarios;
            _mes = mes;
            _inmuebles = inmuebles;
            _lsolicitud = lsolicitud;
            _lentregable = lentregable;
            _aeeProcedure = aeeProcedure;
        }

        [HttpGet]
        public async Task<List<SolicitudPagoDto>> GetAllSolicitudes()
        {
            var solicitudes = await _solicitud.GetAllSolicitudesPago();

            return solicitudes;
        }

        [HttpGet]
        [Route("getSolicitudesByAnio/{anio}")]
        public async Task<List<SolicitudPagoDto>> GetSolicitudesPagoByAnio(int anio)
        {
            var solicitudes = await _solicitud.GetSolicitudesPagoByAnio(anio);
            foreach (var sol in solicitudes)
            {
                sol.Estatus = await _estatussp.GetESPagoByIdAsync(sol.EstatusId);
                sol.Mes = await _mes.GetMesByIdAsync(sol.MesId);
                sol.Inmueble = await _inmuebles.GetInmuebleById(sol.InmuebleId);
            }
            return solicitudes;
        }

        [HttpGet]
        [Route("getSolicitudById/{solicitud}")]
        public async Task<SolicitudPagoDto> GetSolicitudPagoById(int solicitud)
        {
            var solicitudes = await _solicitud.GetSolicitudPagoById(solicitud);

            solicitudes.Entregables = await _entregables.GetEntregablesBySolicitud(solicitud);
            solicitudes.Estatus = await _estatussp.GetESPagoByIdAsync(solicitudes.EstatusId);
            solicitudes.Flujo = await _estatussp.GetFlujoESPagoAsync(solicitudes.ServicioId, solicitudes.EstatusId);
            solicitudes.Mes = await _mes.GetMesByIdAsync(solicitudes.MesId);
            solicitudes.Inmueble = await _inmuebles.GetInmuebleById(solicitudes.InmuebleId);
            solicitudes.Usuario = await _usuarios.GetUsuarioByIdAsync(solicitudes.UsuarioId);
            solicitudes.CFDIs = await _cfdi.GetFacturasBySolicitudAsync(solicitud);
            solicitudes.LogS = await _lsolicitud.GetHistorialBySolicitud(solicitud);
            solicitudes.LogE = await _lentregable.GetHistorialEBySolicitud(solicitud);

            foreach (var f in solicitudes.Flujo)
            {
                f.Permiso = (await _estatussp.GetESPagoByIdAsync(f.ESucesivoId)).Abreviacion;
            }
            
            foreach (var f in solicitudes.CFDIs)
            {
                f.Conceptos = await _cfdi.GetConceptosFacturaByIdAsync(f.Id);
                f.Axa = await _cfdi.GetASGeneralesByIdAsync(f.Id);
            }
                    
            foreach (var ls in solicitudes.LogS)
            {
                ls.Estatus = await _estatussp.GetESPagoByIdAsync(ls.EstatusId);
                ls.Usuario = await _usuarios.GetUsuarioByIdAsync(ls.UsuarioId);
            }
            
            foreach (var le in solicitudes.LogE)
            {
                le.Estatus = await _estatuse.GetEEByIdAsync(le.EstatusId);
                le.Usuario = await _usuarios.GetUsuarioByIdAsync(le.UsuarioId);
                le.Entregable = await _ctentregables.GetEntregableById(le.EntregableId);
            }

            foreach (var en in solicitudes.Entregables)
            {
                en.Entregable = await _ctentregables.GetEntregableById(en.EntregableId);
                en.Estatus = await _estatuse.GetEEByIdAsync(en.EstatusId);
            }

            return solicitudes;
        }

        [HttpPost]
        [Route("createSolicitud")]
        public async Task<IActionResult> CreateSolicitudPago([FromBody] SolicitudPagoCreateCommand solicitud)
        {
            int status = await _solicitud.CreateSolicitud(solicitud);

            return Ok(status);
        }
        
        [HttpPut]
        [Route("updateSolicitud")]
        public async Task<IActionResult> UpdateSolicitudPago([FromBody] SolicitudPagoUpdateCommand solicitud)
        {
            int status = await _solicitud.UpdateSolicitud(solicitud);

            if (status == 201)
            {
                var estatusE = await _aeeProcedure.EnviarSolicitudP(solicitud.EstatusId);
                List<EntregableSBDto> entregable = await _entregables.GetEntregablesBySolicitud(solicitud.Id);

                foreach (var en in entregable)
                {
                    en.Estatus = await _estatuse.GetEEByIdAsync(en.EstatusId);
                    if (en.Estatus.Nombre.Equals("En Proceso"))
                    {
                        var update = new EntregableSBUpdateCommand
                        {
                            Id = en.Id,
                            SolicitudId = solicitud.Id,
                            UsuarioId = solicitud.UsuarioId,
                            EntregableId = en.EntregableId,
                            EstatusId = estatusE,
                            TipoEntregable = "",
                            Observaciones = en.Observaciones
                        };

                        await _entregables.UpdateEntregable(update);
                    }                    
                }
            }

            return Ok(status);
        }
    }
}
