using Api.Gateway.Models.Entregables.ServiciosBasicos.Commands;
using Api.Gateway.Models.Entregables.ServiciosBasicos.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.CFDIs;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.Entregables;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.SolicitudesPagos;
using Api.Gateway.WebClient.Procedures.ServiciosBasicos.AEElectrica;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.ServiciosBasicos.AEElectrica.Entregables
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("aeelectrica/entregables")]
    public class AEEEntregableController : ControllerBase
    {
        private readonly IAEESolicitudPagoProxy _solicitud;
        private readonly IAEEEntregableProxy _entregables;
        private readonly IMesProxy _mes;
        private readonly IAEECFDIProxy _cfdi;
        private readonly IEstatusEntregableProxy _estatuse;
        private readonly ICTEntregableProxy _ctentregable;

        //Procedures
        private readonly IAEElectricaProcedure _aeeProcedure;

        public AEEEntregableController(IAEESolicitudPagoProxy solicitud, IAEEEntregableProxy entregables, IMesProxy mes, IAEECFDIProxy cfdi, 
                                       IEstatusEntregableProxy estatuse, ICTEntregableProxy ctentregable, IAEElectricaProcedure aeeProcedure)
        {
            _solicitud = solicitud;
            _entregables = entregables;
            _mes = mes;
            _cfdi = cfdi;
            _estatuse = estatuse;
            _ctentregable = ctentregable;
            _aeeProcedure = aeeProcedure;
        }

        public async Task<List<EntregableSBDto>> GetAllEntregables()
        {
            var entregables = await _entregables.GetAllEntregables();

            return entregables;
        }

        [HttpGet]
        [Route("getEntregablesBySolicitud/{solicitud}")]
        public async Task<List<EntregableSBDto>> GetEntregablesBySolicitud(int solicitud)
        {
            var entregables = await _entregables.GetEntregablesBySolicitud(solicitud);

            return entregables;
        }

        [HttpGet]
        [Route("getEntregableById/{entregable}")]
        public async Task<EntregableSBDto> GetEntregableById(int entregable)
        {
            var entregables = await _entregables.GetEntregableById(entregable);

            return entregables;
        }

        [HttpPut]
        [Route("updateEntregable")]
        public async Task<IActionResult> UpdateEntregable([FromForm] EntregableSBUpdateCommand entregable)
        {
            entregable.EstatusId = await _aeeProcedure.GetEstatusEntregable(entregable.SolicitudId);
            entregable.EntregableId = (await _ctentregable.GetAllCTEntregables()).SingleOrDefault(e => e.Nombre.Equals(entregable.TipoEntregable)).Id;
            int status = await _entregables.UpdateEntregable(entregable);
            return Ok(status);
        }
        
        //Autorizar o rechazar el entregable
        [HttpPut]
        [Route("seguimientoEntregable")]
        public async Task<IActionResult> UpdateEntregable([FromBody] AEntregableSBUpdateCommand entregable)
        {
            entregable.EstatusId = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(e => e.Nombre.Equals(entregable.Estatus)).Id;
            int status = await _entregables.SeguimientoEntregable(entregable);
            return Ok(status);
        }


        [HttpGet("visualizarEntregable/{idSol}/{tipo}/{archivo}")]
        public async Task<string> VisualizarEntregablePDFs(int idSol, string tipo, string archivo)
        {
            var solicitud = await _solicitud.GetSolicitudPagoById(idSol);
            var mes = await _mes.GetMesByIdAsync(solicitud.MesId);
            var factura = (await _cfdi.GetFacturasBySolicitudAsync(idSol)).SingleOrDefault(f => f.ArchivoPDF == archivo);
            string ruta = "";

            if (tipo.Equals("Factura"))
            {
                ruta = "Entregables\\" + solicitud.Anio + "\\" + mes.Nombre + "\\"+tipo+"\\" + (factura.Serie + factura.Folio);
            }
            else
            {
                ruta = "Entregables\\" + solicitud.Anio + "\\" + mes.Nombre + "\\"+tipo;
            }


            var file = await _entregables.VisualizarEntregable(ruta.Replace("\\","¬¬"), archivo);

            return file.ToString();
        }
    }
}
