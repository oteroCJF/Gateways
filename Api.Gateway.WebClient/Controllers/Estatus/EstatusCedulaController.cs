using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusCedulas;
using Api.Gateway.Proxies.Estatus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Estatus
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("estatus/cedulas")]
    public class EstatusCedulaController : ControllerBase
    {
        private readonly IEstatusCedulaProxy _estatus;
        public EstatusCedulaController(IEstatusCedulaProxy estatus)
        {
            _estatus = estatus;
        }

        [HttpGet]

        public async Task<List<EstatusDto>> GetAllEstatusCedulaAsync()
        {
            var result = await _estatus.GetAllEstatusCedulaAsync();

            return result;
        }


        [HttpGet("getECedulaById/{estatus}")]
        public async Task<EstatusDto> GetECByIdAsync(int estatus)
        {
            var result = await _estatus.GetECByIdAsync(estatus);

            return result;
        }
        
        [HttpGet("getFlujoByServicio/{servicio}/{estatusC}/{flujo}")]
        public async Task<List<FlujoServicioDto>> GetFlujoByServicio(int servicio, int estatusC, string flujo)
        {
            var result = await _estatus.GetFlujoByServicio(servicio, estatusC, flujo);

            foreach (var es in result)
            {
                es.ESucesivo = await _estatus.GetECByIdAsync(es.ESucesivoId);
            }

            return result;
        }
    }
}
