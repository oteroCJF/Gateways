using Api.Gateway.Models.Estatus.DTOs;
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
    [Route("estatus/solicitudesPago")]
    public class ESolicitudPagoController : ControllerBase
    {
        private readonly IEstatusSPProxy _estatus;
        public ESolicitudPagoController(IEstatusSPProxy estatus)
        {
            _estatus = estatus;
        }
        [HttpGet]
        public async Task<List<EstatusDto>> GetAllEstatusSPagoAsync()
        {
            return await _estatus.GetAllEstatusSPagoAsync();
        }

        [Route("getSPagoById/{estatus}")]
        [HttpGet]
        public async Task<EstatusDto> GetECByIdAsync(int estatus)
        {
            return await _estatus.GetESPagoByIdAsync(estatus);
        }

        [Route("getESPagoById/{estatus}")]
        [HttpGet]
        public async Task<EstatusDto> GetSPByServicio(int estatus)
        {
            return await _estatus.GetESPagoByIdAsync(estatus);
        }

        [Route("getSPByServicio/{servicio}/{estatus}")]
        [HttpGet]
        public async Task<List<FlujoBasicosDto>> GetSPByServicio(int servicio, int estatus)
        {
            return await _estatus.GetFlujoESPagoAsync(servicio, estatus);
        }
    }
}
