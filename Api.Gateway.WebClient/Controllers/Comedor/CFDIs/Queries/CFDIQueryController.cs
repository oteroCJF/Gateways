using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Comedor.CFDIs.Queries;
using Api.Gateway.Proxies.Comedor.Contratos.Queries;
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

namespace Api.Gateway.WebClient.Controllers.Comedor.CFDIs.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/cfdi")]
    public class CFDIQueryController : ControllerBase
    {
        private readonly IQRepositorioComedorProxy _repositorio;
        private readonly IQCFDIComedorProxy _facturas;
        private readonly IQContratoComedorProxy _contrato;
        private readonly IUsuarioProxy _usuarios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IMesProxy _mes;
        private readonly ICFDIProcedure _cfdip;
        private readonly IEstatusFacturaProxy _estatusf;

        public CFDIQueryController(IQRepositorioComedorProxy repositorio, IQCFDIComedorProxy facturas, IQContratoComedorProxy contrato, IUsuarioProxy usuarios, 
                                   IInmuebleProxy inmuebles, IMesProxy mes, ICFDIProcedure cfdip, IEstatusFacturaProxy estatusf)
        {
            _repositorio = repositorio;
            _facturas = facturas;
            _contrato = contrato;
            _usuarios = usuarios;
            _inmuebles = inmuebles;
            _mes = mes;
            _cfdip = cfdip;
            _estatusf = estatusf;
        }

        [Route("getFacturasByRepositorio/{repositorio}")]
        [HttpGet]
        public async Task<List<CFDIDto>> GetAllFacturas(int repositorio)
        {
            var facturas = await _facturas.GetAllFacturasAsync(repositorio);
            foreach (var f in facturas)
            {
                f.Usuario = await _usuarios.GetUsuarioByIdAsync(f.UsuarioId);
                f.Inmueble = await _inmuebles.GetInmuebleById(f.InmuebleId);
                f.ConceptosFactura = await _facturas.GetConceptosFacturaByIdAsync(f.Id);
            }

            return facturas;
        }

        [Route("getFacturasByInmueble/{inmueble}/{repositorio}")]
        [HttpGet]
        public async Task<List<CFDIDto>> GetFacturasByInmueble(int inmueble, int repositorio)
        {
            var facturas = await _facturas.GetFacturasByInmuebleAsync(inmueble, repositorio);

            foreach (var f in facturas)
            {
                f.Usuario = await _usuarios.GetUsuarioByIdAsync(f.UsuarioId);
                f.Estatus = await _estatusf.GetEFByIdAsync(f.EstatusId);
                f.Inmueble = await _inmuebles.GetInmuebleById(f.InmuebleId);
                f.ConceptosFactura = await _facturas.GetConceptosFacturaByIdAsync(f.Id);
            }
            return facturas;
        }

        [HttpGet("getXMLInmueble/{inmueble}/{repositorio}")]
        public async Task<List<CFDIDto>> GetRepositorio(int inmueble, int repositorio)
        {
            return await _facturas.GetFacturasByInmuebleAsync(inmueble, repositorio);
        }

        [Route("getHistorialMFByRepositorio/{repositorio}")]
        [HttpGet]
        public async Task<List<HistorialMFDto>> GetHistorialMFByRepositorio(int repositorio)
        {
            var historial = await _facturas.GetHistorialMFByFacturacion(repositorio);
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
