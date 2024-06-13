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
    [Route("estatus/facturas")]
    public class EstatusFacturaController : ControllerBase
    {
        private readonly IEstatusFacturaProxy _estatus;
        public EstatusFacturaController(IEstatusFacturaProxy estatus)
        {
            _estatus = estatus;
        }

        [HttpGet]

        public async Task<List<EstatusDto>> GetAllEstatusFacturasAsync()
        {
            var result = await _estatus.GetAllEstatusFacturasAsync();

            return result;
        }

        [HttpGet("getEFacturaById/{estatus}")]
        public async Task<EstatusDto> GetEFById(int estatus)
        {
            var result = await _estatus.GetEFByIdAsync(estatus);

            return result;
        }
    }
}
