using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Limpieza;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Repositorios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using Api.Gateway.Proxies.Limpieza.Facturas;
using Api.Gateway.Proxies.Limpieza.Oficios;
using Api.Gateway.Proxies.Limpieza.Repositorios;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.Oficios.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/oficios")]
    public class OficioController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly ICTServicioProxy _servicios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly ILCedulaProxy _cedula;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly IEstatusOficioProxy _estatuso;
        private readonly IEstatusFacturaProxy _estatusf;
        private readonly ILRepositorioProxy _repositorios;
        private readonly ILOficioProxy _oficios;
        private readonly ILCFDIProxy _facturas;

        public OficioController(IMesProxy meses, ICTServicioProxy servicios, IInmuebleProxy inmuebles, ILCedulaProxy cedula, IEstatusCedulaProxy estatusc, 
                                IEstatusOficioProxy estatuso, IEstatusFacturaProxy estatusf, ILRepositorioProxy repositorios, ILOficioProxy oficios, ILCFDIProxy facturas)
        {
            _meses = meses;
            _servicios = servicios;
            _inmuebles = inmuebles;
            _cedula = cedula;
            _estatusc = estatusc;
            _estatuso = estatuso;
            _estatusf = estatusf;
            _repositorios = repositorios;
            _oficios = oficios;
            _facturas = facturas;
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

            var facturas = detalle.GroupJoin(fac, d => d.FacturaId, f => f.Id, (x, y) => new { detalle = x, fac = y })
                .SelectMany(x => x.fac.DefaultIfEmpty(), (x, y) => new { x.detalle, fac = y })
                .GroupJoin(cedulas, d => d.detalle.CedulaId, c => c.Id, (x, y) => new { detalle = x, cedulas = y })
                .SelectMany(x => x.cedulas.DefaultIfEmpty(), (x, z) => new { x.detalle, cedulas = z })
                .Select(async dt => new FCFDIDto
                {
                    Id = dt.detalle.fac.Id,
                    CedulaId = dt.cedulas.Id,
                    RepositorioId = dt.detalle.fac.RepositorioId,
                    InmuebleId = dt.cedulas.InmuebleId,
                    EstatusId = dt.cedulas.EstatusId,
                    Estatus = await GetEstatusCedulaId(dt.cedulas.EstatusId),
                    EFactura = await GetEstatusFacturaId(dt.detalle.fac.EstatusId),
                    Inmueble = (await GetInmuebleId(dt.cedulas.InmuebleId)).Nombre,
                    Tipo = dt.detalle.fac.Tipo,
                    Facturacion = dt.detalle.fac.Facturacion,
                    RFC = dt.detalle.fac.RFC,
                    Nombre = dt.detalle.fac.Nombre,
                    Serie = dt.detalle.fac.Serie,
                    FolioFactura = dt.detalle.fac.Folio,
                    FolioCedula = dt.cedulas.Folio,
                    Calificacion = dt.cedulas.Calificacion,
                    UsoCFDI = dt.detalle.fac.UsoCFDI,
                    UUID = dt.detalle.fac.UUID,
                    UUIDRelacionado = dt.detalle.fac.UUIDRelacionado,
                    FechaTimbrado = dt.detalle.fac.FechaTimbrado,
                    IVA = dt.detalle.fac.IVA,
                    Subtotal = dt.detalle.fac.Subtotal,
                    Total = dt.detalle.fac.Total,
                    FechaCreacion = dt.detalle.fac.FechaCreacion
                    })
                    .Select(r => r.Result)
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

            var facturas = (await _facturas.GetFacturasNCPendientes(estatusId)).Where(f => !facturasId.Contains(f.Id)).Select(async f => new CFDIDto
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
                //Cedula = await _mpcfdi.GetCedulaEvaluacion(f.RepositorioId, f.InmuebleId)
            }).Select(f => f.Result).ToList();

            return facturas;

        }

        private async Task<RepositorioDto> GetRepositorioId(int id)
        {
            var facturacion = await _repositorios.GetRepositorioByIdAsync(id);
            return facturacion;
        }

        private async Task<CedulaLimpiezaDto> GetCedulaId(int id, int inmueble)
        {
            var facturacion = await _repositorios.GetRepositorioByIdAsync(id);
            var cedula = await _cedula.GetCedulaById(id);

            return cedula;
        }

        private async Task<MesDto> GetMesId(int id)
        {
            var mes = await _meses.GetMesByIdAsync(id);

            return mes;
        }

        private async Task<InmuebleDto> GetInmuebleId(int id)
        {
            var inmueble = await _inmuebles.GetInmuebleById(id);

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

    }
}
