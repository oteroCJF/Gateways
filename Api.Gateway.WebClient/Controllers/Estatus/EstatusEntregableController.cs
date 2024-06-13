using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Estatus.DTOs.EstatusCedulas;
using Api.Gateway.Models.Estatus.DTOs.EstatusEntregables;
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
    [Route("estatus/entregables")]
    public class EstatusEntregableController : ControllerBase
    {
        private readonly IEstatusEntregableProxy _estatus;
        public EstatusEntregableController(IEstatusEntregableProxy estatus)
        {
            _estatus = estatus;
        }

        [HttpGet]

        public async Task<List<EstatusDto>> GetAllEstatusEntregablesAsync()
        {
            var result = await _estatus.GetAllEstatusEntregablesAsync();

            return result;
        }

        [HttpGet("getEEntregableById/{estatus}")]
        public async Task<EstatusDto> GetEEByIdAsync(int estatus)
        {
            var result = await _estatus.GetEEByIdAsync(estatus);

            return result;
        }

        [HttpGet("getAllFlujoByEntregablesServicio/{servicio}")]
        public async Task<List<FlujoEntregableDto>> GetAllFlujoByEntregablesServicio(int servicio)
        {
            var result = await _estatus.GetFlujoByEntregableServicio(servicio);

            return result;
        }
        
        [HttpGet("getFlujoByEntregablesSE/{servicio}/{estatusC}")]
        public async Task<List<FlujoEntregableDto>> GetFlujoByEntregablesSE(int servicio, int estatusC)
        {
            var result = await _estatus.GetFlujoByEntregablesSE(servicio, estatusC);

            return result;
        }
    }
}
