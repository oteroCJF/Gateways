using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Fumigacion;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Oficios.Commands;
using Api.Gateway.Models.Repositorios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Fumigacion.CedulasEvaluacion;
using Api.Gateway.Proxies.Fumigacion.CFDIs;
using Api.Gateway.Proxies.Fumigacion.Facturacion;
using Api.Gateway.Proxies.Fumigacion.Oficios;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Fumigacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Fumigacion.Oficios
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("fumigacion/oficios")]
    public class OficioController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly ICTServicioProxy _servicios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IFCedulaProxy _cedula;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly IEstatusOficioProxy _estatuso;
        private readonly IEstatusFacturaProxy _estatusf;
        private readonly IFRepositorioProxy _facturacion;
        private readonly IFOficioProxy _oficios;
        private readonly IFCFDIProxy _facturas;
        private readonly IFCFDIProcedure _mpcfdi;

        public OficioController(IMesProxy meses, ICTServicioProxy servicios, IInmuebleProxy inmuebles, IFCedulaProxy cedula, 
                                IEstatusCedulaProxy estatusc, IEstatusOficioProxy estatuso, IEstatusFacturaProxy estatusf, 
                                IFRepositorioProxy facturacion, IFOficioProxy oficios, IFCFDIProxy facturas, IFCFDIProcedure mpcfdi)
        {
            _meses = meses;
            _servicios = servicios;
            _inmuebles = inmuebles;
            _cedula = cedula;
            _estatusc = estatusc;
            _estatuso = estatuso;
            _estatusf = estatusf;
            _facturacion = facturacion;
            _oficios = oficios;
            _facturas = facturas;
            _mpcfdi = mpcfdi;
        }

        [HttpGet]
        [Route("getAllOficios")]
        public async Task<IActionResult> GetAllOficiosAsync()
        {
            var oficios = await _oficios.GetAllOficiosAsync();

            return Ok(oficios);
        }        

        [HttpGet]
        [Route("getOficiosByAnio/{anio}")]
        public async Task<IActionResult> GetOficiosByAnio(int anio)
        {
            var oficios = await _oficios.GetOficiosByAnio(anio);

            foreach (var of in oficios)
            {
                of.Estatus = await _estatuso.GetEOficioById((int)of.EstatusId);
            }

            return Ok(oficios);
        }
        
        [HttpGet]
        [Route("getOficioById/{id}")]
        public async Task<IActionResult> GetOficioById(int id)
        {
            var oficio = await _oficios.GetOficioById(id);

            oficio.Estatus = await _estatuso.GetEOficioById((int)oficio.EstatusId);

            oficio.CFDIs = await GetDetalleOficio(id);

            return Ok(oficio);
        }

        public async Task<List<FCFDIDto>> GetDetalleOficio(int oficio)
        {
            var oficios = await _oficios.GetOficioById(oficio);
            var detalle = await _oficios.GetDetalleOficio(oficio);
            var cedulasId = detalle.Select(dt => dt.CedulaId).Distinct().ToList();
            var facturasId = detalle.Select(dt => dt.FacturaId).Distinct().ToList();
            var cedulas = (await _cedula.GetCedulaEvaluacionByAnio((int)oficios.Anio)).Where(c => cedulasId.Contains(c.Id))
                          .OrderBy(c => c.Id).ToList();
            var fac = (await _facturas.GetAllFacturas()).Where(f => facturasId.Contains(f.Id)).ToList();

            var facturas = fac.Select(async f => new FCFDIDto { 
                               Id = f.Id,
                               CedulaId = (await GetCedulaId(f.RepositorioId, f.InmuebleId)).Id,
                               RepositorioId = f.RepositorioId,
                               InmuebleId = f.InmuebleId,
                               EstatusId = (await GetCedulaId(f.RepositorioId, f.InmuebleId)).EstatusId,
                               Estatus = (await GetEstatusCedulaId((await GetCedulaId(f.RepositorioId, f.InmuebleId)).EstatusId)),
                               EFactura = (await GetEstatusFacturaId(f.EstatusId)),
                               Inmueble = (await GetInmuebleId(f.InmuebleId)).Nombre,
                               Tipo = f.Tipo,
                               Facturacion = f.Facturacion,
                               RFC = f.RFC,
                               Nombre = f.Nombre,
                               Serie = f.Serie,
                               FolioFactura = f.Folio,
                               FolioCedula = (await GetCedulaId(f.RepositorioId, f.InmuebleId)).Folio,
                               Calificacion = (await GetCedulaId(f.RepositorioId, f.InmuebleId)).Calificacion,
                               UsoCFDI = f.UsoCFDI,
                               UUID = f.UUID,
                               UUIDRelacionado = f.UUIDRelacionado,
                               FechaTimbrado = f.FechaTimbrado,
                               IVA = f.IVA,
                               Subtotal = f.Subtotal,
                               Total = f.Total,
                               FechaCreacion = f.FechaCreacion
                           })
                           .Select(r => r.Result)
                           .OrderBy(f => f.Id)
                           .ToList();


            return facturas;
        }

        [Route("getFacturasNCPendientes/{oficio}")]
        [HttpGet]
        public async Task<List<CFDIDto>> GetFacturasNCPendientes(int oficio)
        {
            var estatusId = (await _estatusf.GetAllEstatusFacturasAsync()).Single(e => e.Nombre.Equals("Pendiente")).Id;

            var detalle = await _oficios.GetDetalleOficio(oficio);
            var facturasId = detalle.Select(dt => dt.FacturaId).Distinct().ToList();

            var facturas = (await _facturas.GetFacturasNCPendientes(estatusId)).Where(f=> !facturasId.Contains(f.Id)).Select(async f => new CFDIDto
            { 
                Id = f.Id,
                Estatus = await _estatusf.GetEFByIdAsync(f.EstatusId),
                Inmueble = await _inmuebles.GetInmuebleById(f.InmuebleId),
                Tipo = f.Tipo,
                RFC = f.RFC,
                Nombre = f.Nombre,
                Serie = f.Serie,
                Folio = f.Folio,
                UUID = f.UUID,
                UUIDRelacionado = f.UUIDRelacionado,
                FechaTimbrado = f.FechaTimbrado,
                IVA = f.IVA,
                Subtotal = f.Subtotal,
                Total = f.Total,
                Cedula = await _mpcfdi.GetCedulaEvaluacion(f.RepositorioId, f.InmuebleId)
            }).Select(f => f.Result).ToList();

            return facturas;

        }

        private async Task<RepositorioDto> GetRepositorioId(int id)
        {
            var facturacion = await _facturacion.GetFacturacionByIdAsync(id);
            return facturacion;
        }
        
        private async Task<CedulaFumigacionDto> GetCedulaId(int id, int inmueble)
        {
            var facturacion = await _facturacion.GetFacturacionByIdAsync(id);
            var cedula = (await _cedula.GetCedulaEvaluacionByAnioMes(facturacion.Anio, facturacion.MesId))
                         .Single(c => c.InmuebleId == inmueble && facturacion.ContratoId == c.ContratoId);

            return cedula;
        }
        
        private async Task<MesDto> GetMesId(int id)
        {
            var mes = await _meses.GetMesByIdAsync(id);

            return mes;
        }
        
        private async Task<InmuebleDto> GetInmuebleId(int id)
        {
            var inmueble= await _inmuebles.GetInmuebleById(id);

            return inmueble;
        }
        
        private async Task<EstatusDto> GetEstatusCedulaId(int id)
        {
            var estatus = await _estatusc.GetECByIdAsync(id);

            return estatus;
        }
        
        private async Task<EstatusDto> GetEstatusFacturaId(int id)
        {
            var estatus = await _estatusf.GetEFByIdAsync(id);

            return estatus;
        }

        [Consumes("multipart/form-data")]
        [Route("createOficio")]
        [HttpPost]
        public async Task<IActionResult> createOficio([FromForm] OficioCreateCommand request)
        {
            var oficio = await _oficios.CreateOficio(request);

            return Ok(oficio);
        }

        [Route("createDetalleOficio")]
        [HttpPost]
        public async Task<IActionResult> CreateDetalleOficio([FromBody] List<DetalleOficioCreateCommand> request)
        {
            var oficio = await _oficios.CreateDetalleOficio(request);
            return Ok(oficio);
        }

        [Route("deleteDetalleOficio")]
        [HttpPost]
        public async Task<IActionResult> DeleteDetalleOficio([FromBody] DetalleOficioDeleteCommand request)
        {
            var oficio = await _oficios.DeleteDetalleOficio(request);
            return Ok(oficio);
        }
        
        [Route("corregirOficio")]
        [HttpPost]
        public async Task<IActionResult> CorregirOficio([FromBody] CorregirOficioCommand request)
        {
            var oficio = await _oficios.CorregirOficio(request);
            return Ok(oficio);
        }
        
        [Route("eDGPPTOficio")]
        [HttpPost]
        public async Task<IActionResult> EDGPPTOficio([FromBody] EDGPPTOficioCommand request)
        {
            var oficio = await _oficios.EDGPPTOficio(request);
            return Ok(oficio);
        }
        
        [Route("pagarOficio")]
        [HttpPost]
        public async Task<IActionResult> PagarOficio([FromBody] PagarOficioCommand request)
        {
            var oficio = await _oficios.PagarOficio(request);
            return Ok(oficio);
        }
    }
}
