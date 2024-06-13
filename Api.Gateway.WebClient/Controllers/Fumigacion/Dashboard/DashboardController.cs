using Api.Gateway.Models.Dashboard.Cedulas;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Fumigacion.CedulasEvaluacion;
using Api.Gateway.Proxies.Fumigacion.CFDIs;
using Api.Gateway.Proxies.Fumigacion.Facturacion;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Fumigacion.Dashboard
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("fumigacion/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IMesProxy _meses;
        private readonly ICTServicioProxy _servicios;
        private readonly IFCedulaProxy _cedula;
        private readonly IFRepositorioProxy _repositorios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly IFCFDIProxy _facturas;

        public DashboardController(IMesProxy meses, IFCedulaProxy cedula, IFRepositorioProxy facturacion, IFCFDIProxy facturas,
                                   IInmuebleProxy inmuebles, IEstatusCedulaProxy estatusc, ICTServicioProxy servicios)
        {
            _meses = meses;
            _cedula = cedula;
            _estatusc = estatusc;
            _repositorios = facturacion;
            _facturas = facturas;
            _inmuebles = inmuebles;
            _servicios = servicios;
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
            var repositorio = (await _repositorios.GetAllFacturacionesAsync(anio)).Select(f => f.Id);
            List<CedulaDto> cedulas = new List<CedulaDto>();
            if (repositorio.Count() != 0)
            {
                cedulas = (await _cedula.GetCedulaEvaluacionByAnio(anio))
                            .Where(c => inmuebles.Contains(c.InmuebleId))
                            .GroupBy(f => new { f.EstatusId })
                            .Select(f => new CedulaDto
                            {
                                EstatusId = f.Key.EstatusId,
                                Total = f.Count(),
                                PorcentajeAvance = Convert.ToDecimal(((decimal)f.Count() * (decimal)100) / ((decimal)inmuebles.Count() * meses.Count()))
                            })
                            .ToList();

                    foreach (var cd in cedulas)
                    {
                        cd.Estatus = (await _estatusc.GetECByIdAsync(cd.EstatusId)).Nombre;
                        cd.Fondo = (await _estatusc.GetECByIdAsync(cd.EstatusId)).Fondo;
                        cd.FondoH = (await _estatusc.GetECByIdAsync(cd.EstatusId)).FondoHexadecimal;
                    }
            }

            return cedulas;
        }
    }
}
