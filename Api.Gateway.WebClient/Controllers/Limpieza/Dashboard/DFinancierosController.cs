using Api.Gateway.Models.Dashboard.Financieros;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using Api.Gateway.Proxies.Limpieza.Facturas;
using Api.Gateway.Proxies.Limpieza.Repositorios;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.Dashboard
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/dfinancieros")]
    public class DFinancierosController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly ICTServicioProxy _servicios;
        private readonly ILRepositorioProxy _repositorios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IEstatusFacturaProxy _estatusf;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly ILCFDIProxy _facturas;
        private readonly ILCedulaProxy _cedula;

        public DFinancierosController(IMesProxy meses, ILRepositorioProxy repositorios, ILCFDIProxy facturas,
                                   IInmuebleProxy inmuebles, IEstatusFacturaProxy estatusf, ICTServicioProxy servicios, 
                                   ILCedulaProxy cedula, IEstatusCedulaProxy estatusc)
        {
            _meses = meses;
            _estatusf = estatusf;
            _estatusc = estatusc;
            _repositorios = repositorios;
            _facturas = facturas;
            _inmuebles = inmuebles;
            _servicios = servicios;
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

            List<FacturaDto> facturas = (await _facturas.GetAllFacturas())
                            .Where(fc => repositorios.Contains(fc.RepositorioId) && fc.Tipo.Equals("Factura"))
                            .GroupBy(f => new { f.Tipo })
                            .Select(f => new FacturaDto
                            {
                                Anio = anio,
                                Cargadas = f.Count(),
                                Pendientes = facturasPendientes,
                                PorcentajeAvance = Convert.ToDecimal(((decimal)f.Count() - (decimal)facturasPendientes) / (decimal)f.Count()) * (decimal)100
                            })
                            .ToList();

            return facturas.First();
        }

        private async Task<FacturaDto> PorcentajeNCFinancieros(int anio, int servicio, string usuario)
        {
            var facturacion = (await _repositorios.GetAllRepositoriosAsync(anio)).Select(f => f.Id);
            var estatus = (await _estatusf.GetAllEstatusFacturasAsync()).SingleOrDefault(f => f.Nombre.Equals("Pendiente")).Id;
            var facturasPendientes = (await _facturas.GetAllFacturas())
                            .Where(fc => facturacion.Contains(fc.RepositorioId) && fc.Tipo.Equals("NC") && fc.EstatusId == estatus).Count();

            List<FacturaDto> facturas = (await _facturas.GetAllFacturas())
                            .Where(fc => facturacion.Contains(fc.RepositorioId) && fc.Tipo.Equals("NC"))
                            .GroupBy(f => new { f.Tipo })
                            .Select(f => new FacturaDto
                            {
                                Anio = anio,
                                Cargadas = f.Count(),
                                Pendientes = facturasPendientes,
                                PorcentajeAvance = Convert.ToDecimal(((decimal)f.Count() - (decimal)facturasPendientes) / (decimal)f.Count()) * (decimal)100
                            })
                            .ToList();

            return facturas.First();
        }

        [Route("detalle/{anio}/{servicio}/{usuario}")]
        public async Task<List<DetalleServicioDto>> PorcentajeAvanceCedula(int anio, int servicio, string usuario)
        {
            var inmuebles = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId);
            var meses = await _meses.GetAllMesesAsync();
            var estatus = await _estatusc.GetAllEstatusCedulaAsync();

            var cedulas = (await _cedula.GetCedulaEvaluacionByAnio(anio))
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
