using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Incidencias.Limpieza.Commands;
using Api.Gateway.Models.Incidencias.Limpieza.DTOs;
using Api.Gateway.Models.Incidencias.Mensajeria.Commands;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using Api.Gateway.Models.Inmuebles.DTOs.Parametros;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Mensajeria.SoportePago.Commands;
using Api.Gateway.Proxies.Mensajeria.SoportePago.Queries;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.SoportePago.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/soportePago")]
    public class SoportePagoCommandController : ControllerBase
    {
        private readonly ICSoportePagoMensajeriaProxy _soporte;
        private readonly IQCedulaMensajeriaProxy _cedulas;
        private readonly IInmuebleProxy _inmuebles;

        public SoportePagoCommandController(ICSoportePagoMensajeriaProxy soporte, IInmuebleProxy inmuebles, IQCedulaMensajeriaProxy cedulas)
        {
            _soporte = soporte;
            _inmuebles = inmuebles;
            _cedulas = cedulas;
        }


        [Consumes("multipart/form-data")]
        [Route("insertaSoportePago")]
        [HttpPost]
        public async Task<IActionResult> InsertaSoportePago([FromForm] MSoportePagoCreateCommand soporte)
        {
            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            var cedulas = (await _cedulas.GetCedulaEvaluacionByAnio(soporte.Anio)).Items.Where(cedulas => soporte.MesId == cedulas.MesId);

            soporte.Cedulas = cedulas.GroupJoin(inmuebles,
                                           c => c.InmuebleId,
                                           i => i.Id,
                                           (x, y) => new { cedulas = x, inmuebles = y })
                                           .SelectMany(
                                                x => x.inmuebles.DefaultIfEmpty(), (x, y) => new { x.cedulas, inmuebles = y }
                                            )
                                           .Select(sp => new MCedulaSoporteCommand
                                           {
                                               Id = sp.cedulas.Id,
                                               Cliente = sp.inmuebles.ClienteEstafeta
                                           })
                                           .ToList();

            soporte.Folio = soporte.Folio.Replace("/", "_");

            var incidencias = await _soporte.CreateSoportePago(soporte);

            return Ok(incidencias);
        }

        [Consumes("multipart/form-data")]
        [Route("actualizaSoportePago")]
        [HttpPost]
        public async Task<IActionResult> ActualizaSoportePago([FromForm] MSoportePagoUpdateCommand soporte)
        {
            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            var cedulas = (await _cedulas.GetCedulaEvaluacionByAnio(soporte.Anio)).Items.Where(cedulas => soporte.MesId == cedulas.MesId);

            soporte.Cedulas = cedulas.GroupJoin(inmuebles,
                                           c => c.InmuebleId,
                                           i => i.Id,
                                           (x, y) => new { cedulas = x, inmuebles = y })
                                           .SelectMany(
                                                x => x.inmuebles.DefaultIfEmpty(), (x, y) => new { x.cedulas, inmuebles = y }
                                            )
                                           .Select(sp => new MCedulaSoporteCommand
                                           {
                                               Id = sp.cedulas.Id,
                                               Cliente = sp.inmuebles.ClienteEstafeta
                                           })
                                           .ToList();

            soporte.Folio = soporte.Folio.Replace("/", "_");

            var incidencias = await _soporte.ActualizaSoportePago(soporte);

            return Ok(incidencias);
        }
    }
}
