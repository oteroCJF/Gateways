using Api.Gateway.Models.Dashboard.Cedulas;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Transporte.CedulasEvaluacion;
using Api.Gateway.Proxies.Transporte.CFDIs.Queries;
using Api.Gateway.Proxies.Transporte.Repositorios.Queries;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Transporte.Dashboard
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("transporte/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly ICTServicioProxy _servicios;
        private readonly IQCedulaTransporteProxy _cedula;
        private readonly IQRepositorioTransporteProxy _repositorios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly IQCFDITransporteProxy _facturas;

        public DashboardController(IMesProxy meses, ICTServicioProxy servicios, IQCedulaTransporteProxy cedula, IQRepositorioTransporteProxy repositorios, 
                                   IInmuebleProxy inmuebles, IEstatusCedulaProxy estatusc, IQCFDITransporteProxy facturas)
        {
            _meses = meses;
            _servicios = servicios;
            _cedula = cedula;
            _repositorios = repositorios;
            _inmuebles = inmuebles;
            _estatusc = estatusc;
            _facturas = facturas;
        }

        [HttpGet]
        [Route("index/{anio}/{servicio}/{usuario}")]
        public async Task<IActionResult> Dashboard(int anio, int servicio, string usuario)
        {
            DashboardDto index = new DashboardDto();
            index.DServicio.Servicio = await _servicios.GetServicioByIdAsync(servicio); 
            index.DServicio.Cedulas = await PorcentajeAvanceCedula(anio, servicio, usuario);

            return Ok(index);
        }

        private async Task<List<CedulaDto>> PorcentajeAvanceCedula(int anio, int servicio, string usuario)
        {
            var inmuebles = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId);
            var meses = await _meses.GetAllMesesAsync();
            var repositorios = (await _repositorios.GetAllRepositoriosAsync(anio)).Select(f => f.Id);
            List<CedulaDto> cedulas = (await _cedula.GetCedulaEvaluacionByAnio(anio)).Items
                            .Where(c => inmuebles.Contains(c.InmuebleId))
                            .GroupBy(f => new { f.EstatusId })
                            .Select(f => new CedulaDto
                            {
                                EstatusId = f.Key.EstatusId,
                                Total = f.Count(),
                                PorcentajeAvance = Convert.ToDecimal(((decimal)f.Count() * (decimal)100)/((decimal)inmuebles.Count() * meses.Count()))
                            })
                            .ToList();

            foreach (var cd in cedulas)
            {
                cd.Estatus = (await _estatusc.GetECByIdAsync(cd.EstatusId)).Nombre;
                cd.Fondo = (await _estatusc.GetECByIdAsync(cd.EstatusId)).Fondo;
                cd.FondoH = (await _estatusc.GetECByIdAsync(cd.EstatusId)).FondoHexadecimal;
            }

            return cedulas;
        }

        [HttpGet]
        [Route("detalle/{estatus}/{anio}/{servicio}/{usuario}")]
        public async Task<IActionResult> DDetalleCedulas(int estatus, int anio, int servicio, string usuario)
        {
            var inmueblesId = (await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio)).Select(i => i.InmuebleId);
            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            var meses = await _meses.GetAllMesesAsync();
            List<CedulaDto> cedulas = (await _cedula.GetCedulaEvaluacionByAnio(anio)).Items
                            .Where(c => inmueblesId.Contains(c.InmuebleId) && c.EstatusId == estatus)
                            .GroupBy(f => new { f.InmuebleId, f.EstatusId, f.MesId })
                            .Select(f => new CedulaDto
                            {
                                EstatusId = f.Key.EstatusId,
                                MesId = f.Key.MesId,
                                Mes = meses.Single(m => m.Id == f.Key.MesId).Nombre,
                                InmuebleId = f.Key.InmuebleId,
                                Inmueble = inmuebles.Single( i => i.Id == f.Key.InmuebleId).Nombre,
                                Total = f.Count(),
                                PorcentajeAvance = Convert.ToDecimal(((decimal)f.Count() * (decimal)100) / ((decimal)inmuebles.Count() * meses.Count()))
                            })
                            .OrderBy(o => o.MesId)
                            .ToList();

            foreach (var cd in cedulas)
            {
                cd.Estatus = (await _estatusc.GetECByIdAsync(cd.EstatusId)).Nombre;
                cd.Fondo = (await _estatusc.GetECByIdAsync(cd.EstatusId)).Fondo;
                cd.FondoH = (await _estatusc.GetECByIdAsync(cd.EstatusId)).FondoHexadecimal;
            }

            return Ok(cedulas);
        }
    }
}
