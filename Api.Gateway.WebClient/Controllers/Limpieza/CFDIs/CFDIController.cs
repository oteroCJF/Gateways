using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.Contratos;
using Api.Gateway.Proxies.Limpieza.Facturas;
using Api.Gateway.Proxies.Limpieza.Repositorios;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.CFDI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.CFDIs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/cfdi")]
    public class CFDIController : ControllerBase
    {
        private readonly ILRepositorioProxy _repositorios;
        private readonly ILCFDIProxy _facturas;
        private readonly ILContratoProxy _contrato;
        private readonly IUsuarioProxy _usuarios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IMesProxy _mes;
        private readonly ICFDIProcedure _cfdip;
        private readonly IEstatusFacturaProxy _estatusf;

        public CFDIController(ILRepositorioProxy repositorios, ILCFDIProxy facturas, ILContratoProxy contrato, IUsuarioProxy usuarios, 
                              IInmuebleProxy inmuebles, IMesProxy mes, ICFDIProcedure cfdip, IEstatusFacturaProxy estatusf)
        {
            _repositorios = repositorios;
            _facturas = facturas;
            _contrato = contrato;
            _usuarios = usuarios;
            _inmuebles = inmuebles;
            _mes = mes;
            _cfdip = cfdip;
            _estatusf = estatusf;
        }

        [Route("getFacturasByFacturacion/{facturacion}")]
        [HttpGet]
        public async Task<List<CFDIDto>> GetAllFacturas(int facturacion)
        {
            var facturas = await _facturas.GetAllFacturasAsync(facturacion);
            foreach (var f in facturas)
            {
                f.Usuario = await _usuarios.GetUsuarioByIdAsync(f.UsuarioId);
                f.Inmueble = await _inmuebles.GetInmuebleById(f.InmuebleId);
                f.ConceptosFactura = await _facturas.GetConceptosFacturaByIdAsync(f.Id);
            }

            return facturas;
        }

        [Route("getFacturasByInmueble/{inmueble}/{facturacion}")]
        [HttpGet]
        public async Task<List<CFDIDto>> GetFacturasByInmueble(int inmueble, int facturacion)
        {
            var facturas = await _facturas.GetFacturasByInmuebleAsync(inmueble, facturacion);

            foreach (var f in facturas)
            {
                f.Usuario = await _usuarios.GetUsuarioByIdAsync(f.UsuarioId);
                f.Estatus = await _estatusf.GetEFByIdAsync(f.EstatusId);
                f.Inmueble = await _inmuebles.GetInmuebleById(f.InmuebleId);
                f.ConceptosFactura = await _facturas.GetConceptosFacturaByIdAsync(f.Id);
            }
            return facturas;
        }

        [HttpGet("getXMLInmueble/{inmueble}/{facturacion}")]
        public async Task<List<CFDIDto>> GetFacturacion(int inmueble, int facturacion)
        {
            return await _facturas.GetFacturasByInmuebleAsync(inmueble, facturacion);
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPost("createFactura")]
        public async Task<IActionResult> Create([FromForm] CFDICreateCommand command)
        {
            var factura = await _facturas.CreateFactura(command);
            if (command.XML != null && command.PDF == null)
            {
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
            }

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
            historial.RepositorioId = factura.RepositorioId;
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

        [Route("getHistorialMFByRepositorio/{id}")]
        [HttpGet]
        public async Task<List<HistorialMFDto>> GetHistorialMFByRepositorio(int id)
        {
            var historial = await _facturas.GetHistorialMFByRepositorio(id);
            foreach (var hs in historial)
            {
                var inmueble = await _inmuebles.GetInmuebleById(hs.InmuebleId);
                hs.Inmueble = inmueble.Nombre;
                var usuario = await _usuarios.GetUsuarioByIdAsync(hs.UsuarioId);
                hs.Usuario = usuario.NombreEmp + " " + usuario.PaternoEmp + " " + usuario.MaternoEmp;
            }
            return historial;
        }


        [Route("visualizarFactura/{anio}/{mes}/{folio}/{tipo}/{inmueble}/{archivo}")]
        [HttpGet]
        public async Task<string> VisualizarFactura(int anio, string mes, string folio, string tipo, string inmueble, string archivo)
        {
            var path = await _facturas.VisualizarFactura(anio, mes, folio, tipo, inmueble, archivo);

            return path;
        }
    }
}
