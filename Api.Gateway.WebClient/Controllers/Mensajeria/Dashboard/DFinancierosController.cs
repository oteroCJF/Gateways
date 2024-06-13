using Api.Gateway.Models.Dashboard.Financieros;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion;
using Api.Gateway.Proxies.Mensajeria.CFDIs.Queries;
using Api.Gateway.Proxies.Mensajeria.Repositorios.Queries;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Dashboard
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/dfinancieros")]
    public class DFinancierosController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly ICTServicioProxy _servicios;
        private readonly IQRepositorioMensajeriaProxy _repositorios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IEstatusFacturaProxy _estatusf;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly IQCFDIMensajeriaProxy _facturas;
        private readonly IQCedulaMensajeriaProxy _cedula;

        public DFinancierosController(IMesProxy meses, ICTServicioProxy servicios, IQRepositorioMensajeriaProxy repositorios, IInmuebleProxy inmuebles, 
                                      IEstatusFacturaProxy estatusf, IEstatusCedulaProxy estatusc, IQCFDIMensajeriaProxy facturas, 
                                      IQCedulaMensajeriaProxy cedula)
        {
            _meses = meses;
            _servicios = servicios;
            _repositorios = repositorios;
            _inmuebles = inmuebles;
            _estatusf = estatusf;
            _estatusc = estatusc;
            _facturas = facturas;
            _cedula = cedula;
        }

        [HttpGet]
        [Route("index/{anio}/{servicio}/{usuario}")]
        public async Task<IActionResult> DashboardFinancieros(int anio, int servicio, string usuario)
        {
            DFinancierosDto index = new DFinancierosDto();
            index.Servicio = await _servicios.GetServicioByIdAsync(servicio);
            index.Facturas = await PorcentajeFacturasFinancieros(anio, servicio, usuario);
            index.NC = await PorcentajeNCFinancieros(anio, servicio, usuario);

            return Ok(index);
        }

        private async Task<FacturaDto> PorcentajeFacturasFinancieros(int anio, int servicio, string usuario)
        {
            var repositorios = (await _repositorios.GetAllRepositoriosAsync(anio)).Select(f => f.Id);
            var estatus = (await _estatusf.GetAllEstatusFacturasAsync()).SingleOrDefault(f => f.Nombre.Equals("Pendiente")).Id;
            var facturasPendientes = (await _facturas.GetAllFacturas())
                            .Where(fc => repositorios.Contains(fc.RepositorioId) && fc.Tipo.Equals("Factura") && fc.EstatusId == estatus).Count();
            
            FacturaDto facturas = new FacturaDto();

            facturas = (await _facturas.GetAllFacturas())
                            .Where(fc => repositorios.Contains(fc.RepositorioId) && fc.Tipo.Equals("Factura"))
                            .GroupBy(f => new { f.Tipo })
                            .Select(f => new FacturaDto
                            {
                                Anio = anio,
                                Cargadas = f.Count(),
                                Pendientes = facturasPendientes,
                                PorcentajeAvance = Convert.ToDecimal(((decimal)f.Count() - (decimal)facturasPendientes) / (decimal)f.Count()) * (decimal) 100
                            })
                            .First();

            return facturas;
        }

        private async Task<FacturaDto> PorcentajeNCFinancieros(int anio, int servicio, string usuario)
        {
            var facturacion = (await _repositorios.GetAllRepositoriosAsync(anio)).Select(f => f.Id);
            var estatus = (await _estatusf.GetAllEstatusFacturasAsync()).SingleOrDefault(f => f.Nombre.Equals("Pendiente")).Id;
            var facturasPendientes = (await _facturas.GetAllFacturas())
                            .Where(fc => facturacion.Contains(fc.RepositorioId) && fc.Tipo.Equals("NC") && fc.EstatusId == estatus).Count();

            FacturaDto facturas = new FacturaDto();

            facturas = (await _facturas.GetAllFacturas())
                            .Where(fc => facturacion.Contains(fc.RepositorioId) && fc.Tipo.Equals("NC"))
                            .GroupBy(f => new { f.Tipo })
                            .Select(f => new FacturaDto
                            {
                                Anio = anio,
                                Cargadas = f.Count(),
                                Pendientes = facturasPendientes,
                                PorcentajeAvance = Convert.ToDecimal(((decimal)f.Count() - (decimal)facturasPendientes) / (decimal)f.Count()) * (decimal)100
                            })
                            .First();

            return facturas;
        }

        [Route("detalle/{anio}/{servicio}/{usuario}")]
        public async Task<List<DetalleServicioDto>> PorcentajeAvanceCedula(int anio, int servicio, string usuario)
        {
            var inmuebles = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId);
            var meses = await _meses.GetAllMesesAsync();
            var estatus = await _estatusc.GetAllEstatusCedulaAsync();

            var cedulas = (await _cedula.GetCedulaEvaluacionByAnio(anio)).Items
                            .Where(c => inmuebles.Contains(c.InmuebleId))
                            .GroupBy(c => new { c.EstatusId, c.MesId })
                            .Select(c => new DetalleServicioDto
                            {
                                EstatusId = c.Key.EstatusId,
                                Estatus = estatus.Single(e => e.Id == c.Key.EstatusId).Nombre,
                                Fondo = estatus.Single(e => e.Id == c.Key.EstatusId).Fondo,
                                FondoHexadecimal = estatus.Single(e => e.Id == c.Key.EstatusId).FondoHexadecimal,
                                AreaEjecutora = estatus.Single(e => e.Id == c.Key.EstatusId).AreaEjecutora,
                                MesId = c.Key.MesId,
                                Mes = meses.Single(m => m.Id == c.Key.MesId).Nombre,
                                Total = c.Count(), 
                                Porcentaje = (decimal)c.Count() * 100 / (decimal)inmuebles.Count(),
                            }).OrderBy(c => c.MesId).ThenBy(c => c.EstatusId).ToList();

            return cedulas;
        }
    }
}
