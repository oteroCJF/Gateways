using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Mensajeria.CFDIs;
using Api.Gateway.Proxies.Mensajeria.CFDIs.Commands;
using Api.Gateway.Proxies.Mensajeria.CFDIs.Queries;
using Api.Gateway.Proxies.Mensajeria.Contratos;
using Api.Gateway.Proxies.Mensajeria.Contratos.Commands;
using Api.Gateway.Proxies.Mensajeria.Contratos.Queries;
using Api.Gateway.Proxies.Mensajeria.Repositorios.Commands;
using Api.Gateway.Proxies.Mensajeria.Repositorios.Queries;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Api.Gateway.WebClient.Controllers.Mensajeria.CFDIs.Procedure;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.CFDI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.CFDIs.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/cfdi")]
    public class CFDIQueryController : ControllerBase
    {
        private readonly IUsuarioProxy _usuarios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IMesProxy _mes;
        private readonly IQRepositorioMensajeriaProxy _repositorios;
        private readonly IQCFDIMensajeriaProxy _cfdi;
        private readonly IEstatusFacturaProxy _estatusFacturas;

        public CFDIQueryController(IUsuarioProxy usuarios, IInmuebleProxy inmuebles, IMesProxy mes, IQRepositorioMensajeriaProxy repositorios, 
                                   IQCFDIMensajeriaProxy cfdi, IEstatusFacturaProxy estatusFacturas)
        {
            _usuarios = usuarios;
            _inmuebles = inmuebles;
            _mes = mes;
            _repositorios = repositorios;
            _cfdi = cfdi;
            _estatusFacturas = estatusFacturas;
        }

        [Route("getAllFacturas")]
        [HttpGet]
        public async Task<List<CFDIDto>> GetAllFacturas()
        {
            var facturas = await _cfdi.GetAllFacturas();
            foreach (var f in facturas)
            {
                f.Usuario = await _usuarios.GetUsuarioByIdAsync(f.UsuarioId);
                f.Inmueble = await _inmuebles.GetInmuebleById(f.InmuebleId);
                //f.ConceptosFactura = await _cfdi.GetConceptosFacturaByIdAsync(f.Id);
            }

            return facturas;
        }

        [Route("getFacturasByRepositorio/{repositorio}")]
        [HttpGet]
        public async Task<List<CFDIDto>> GetAllFacturas(int repositorio)
        {
            var facturas = await _cfdi.GetAllFacturasAsync(repositorio);
            foreach (var f in facturas)
            {
                f.Usuario = await _usuarios.GetUsuarioByIdAsync(f.UsuarioId);
                f.Inmueble = await _inmuebles.GetInmuebleById(f.InmuebleId);
                f.ConceptosFactura = await _cfdi.GetConceptosFacturaByIdAsync(f.Id);
            }

            return facturas;
        }

        [Route("getFacturasByInmueble/{inmueble}/{facturacion}")]
        [HttpGet]
        public async Task<List<CFDIDto>> GetFacturasByInmueble(int inmueble, int facturacion)
        {
            var facturas = await _cfdi.GetFacturasByInmuebleAsync(inmueble, facturacion);

            foreach (var f in facturas)
            {
                f.Usuario = await _usuarios.GetUsuarioByIdAsync(f.UsuarioId);
                f.Estatus = await _estatusFacturas.GetEFByIdAsync(f.EstatusId);
                f.Inmueble = await _inmuebles.GetInmuebleById(f.InmuebleId);
                f.ConceptosFactura = await _cfdi.GetConceptosFacturaByIdAsync(f.Id);
            }
            return facturas;
        }

        [HttpGet("getXMLInmueble/{inmueble}/{facturacion}")]
        public async Task<List<CFDIDto>> GetFacturacion(int inmueble, int facturacion)
        {
            return await _cfdi.GetFacturasByInmuebleAsync(inmueble, facturacion);
        }

        [Route("getHistorialMFByFacturacion/{facturacion}")]
        [HttpGet]
        public async Task<List<HistorialMFDto>> GetHistorialMFByFacturacion(int facturacion)
        {
            var historial = await _cfdi.GetHistorialMFByFacturacion(facturacion);
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
            var path = await _cfdi.VisualizarFactura(anio, mes, folio, tipo, inmueble, archivo);

            return path;
        }
    }
}
