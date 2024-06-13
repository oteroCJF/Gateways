using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Comedor.CFDIs.Commands;
using Api.Gateway.Proxies.Comedor.CFDIs.Queries;
using Api.Gateway.Proxies.Comedor.Contratos.Commands;
using Api.Gateway.Proxies.Comedor.Contratos.Queries;
using Api.Gateway.Proxies.Comedor.Repositorios.Commands;
using Api.Gateway.Proxies.Comedor.Repositorios.Queries;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.CFDI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.CFDIs.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/cfdi")]
    public class CFDICommandController : ControllerBase
    {
        private readonly ICRepositorioComedorProxy _repositorio;
        private readonly ICCFDIComedorProxy _facturas; 
        private readonly ICContratoComedorProxy _contrato;
        private readonly IUsuarioProxy _usuarios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IMesProxy _mes;
        private readonly ICFDIProcedure _cfdip;
        private readonly IEstatusFacturaProxy _estatusf;

        public CFDICommandController(ICRepositorioComedorProxy repositorio, ICCFDIComedorProxy facturas, ICContratoComedorProxy contrato, IUsuarioProxy usuarios,
                                IInmuebleProxy inmuebles, IMesProxy mes, ICFDIProcedure cfdip, IEstatusFacturaProxy estatusf)
        {
            _repositorio = repositorio;
            _facturas = facturas;
            _contrato = contrato;
            _usuarios = usuarios;
            _inmuebles = inmuebles;
            _estatusf = estatusf;
            _estatusf = estatusf;
            _mes = mes;
            _cfdip = cfdip;
        }


        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPost("createFactura")]
        public async Task<IActionResult> Create([FromForm] CFDICreateCommand command)
        {
            var factura = await _facturas.CreateFactura(command);
            HistorialMFCreateCommand historial = new HistorialMFCreateCommand();
            historial.Anio = command.Anio;
            historial.Mes = command.Mes;
            historial.RepositorioId = command.RepositorioId;
            historial.InmuebleId = command.InmuebleId;
            historial.UsuarioId = command.UsuarioId;
            historial.TipoArchivo = factura.Tipo;
            historial.Facturacion = command.TipoFacturacion;
            historial.ArchivoXML = factura.ArchivoXML;
            historial.ArchivoPDF = "";

            historial = _cfdip.GetObservacionesHCM(historial, factura);

            await _facturas.CreateHistorialMF(historial);

            return Ok(factura);
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPut("updateFactura")]
        public async Task<IActionResult> UpdateFactura([FromForm] CFDIUpdateCommand command)
        {
            var factura = await _facturas.UpdateFactura(command);
            HistorialMFCreateCommand historial = new HistorialMFCreateCommand();
            historial.Anio = command.Anio;
            historial.Mes = command.Mes;
            historial.Facturacion = factura.Facturacion;
            historial.InmuebleId = factura.InmuebleId;
            historial.UsuarioId = factura.UsuarioId;
            historial.TipoArchivo = factura.Tipo;
            historial.Facturacion = factura.Facturacion;
            historial.ArchivoXML = "";
            historial.ArchivoPDF = factura.ArchivoPDF;

            historial = _cfdip.GetObservacionesHCM(historial, factura);

            await _facturas.CreateHistorialMF(historial);

            return Ok(factura);
        }

        [Route("createHistorialMF")]
        [HttpPost]
        public async Task<IActionResult> CreateHistorialMF([FromBody] HistorialMFCreateCommand historial)
        {
            await _facturas.CreateHistorialMF(historial);
            return Ok();
        }
    }
}
