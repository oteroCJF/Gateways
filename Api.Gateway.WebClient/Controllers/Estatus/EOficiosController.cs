using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusCedulas;
using Api.Gateway.Models.Estatus.DTOs.EstatusOficios;
using Api.Gateway.Proxies.Estatus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Estatus
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("estatus/oficios")]
    public class EOficiosController : ControllerBase
    {
        private readonly IEstatusOficioProxy _estatus;
        public EOficiosController(IEstatusOficioProxy estatus)
        {
            _estatus = estatus;
        }

        [HttpGet]
        public async Task<List<EstatusDto>> GetAllEstatusOficiosAsync()
        {
            return await _estatus.GetAllEstatusOficiosAsync();
        }

        [Route("getEOficioById/{estatus}")]
        [HttpGet]
        public async Task<EstatusDto> GetECByIdAsync(int estatus)
        {
            return await _estatus.GetEOficioById(estatus);
        }

        [HttpGet("getFlujoByOficio/{servicio}/{estatusO}")]
        public async Task<List<FlujoOficiosDto>> GetFlujoByServicio(int servicio, int estatusO)
        {
            var result = await _estatus.GetFlujoByOficio(servicio, estatusO);

            foreach (var es in result)
            {
                es.ESucesivo = await _estatus.GetEOficioById(es.ESucesivoId);
            }

            return result;
        }
    }
}
