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

namespace Api.Gateway.WebClient.Controllers.Mensajeria.SoportePago.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/soportePago")]
    public class SoportePagoQueryController : ControllerBase
    {
        private readonly IQSoportePagoMensajeriaProxy _soporte;
        private readonly IQCedulaMensajeriaProxy _cedulas;
        private readonly IInmuebleProxy _inmuebles;

        public SoportePagoQueryController(IQSoportePagoMensajeriaProxy soporte, IInmuebleProxy inmuebles, IQCedulaMensajeriaProxy cedulas)
        {
            _soporte = soporte;
            _inmuebles = inmuebles;
            _cedulas = cedulas;
        }

        [Route("getSoportePagoByCedula/{cedula}")]
        [HttpGet]
        public async Task<List<MSoportePagoDto>> GetSoportePagoByCedula(int cedula)
        {
            var soporte = await _soporte.GetSoportePagoByCedula(cedula);

            return soporte;
        }


        [Route("getGuiasPendientes/{cedula}")]
        [HttpGet]
        public async Task<List<MSoportePagoDto>> GetGuiasPendientes(int cedula)
        {
            var soporte = await _soporte.GetGuiasPendientes(cedula);

            return soporte;
        }


        [Route("getSoportePagoById/{soporte}")]
        [HttpGet]
        public async Task<MSoportePagoDto> GetSoportePagoById(int soporte)
        {
            var result = await _soporte.GetSoportePagoById(soporte);

            return result;
        }
    }
}
