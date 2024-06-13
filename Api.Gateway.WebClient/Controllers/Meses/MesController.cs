using Api.Gateway.Models;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Proxies.Meses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Meses
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("meses")]
    public class MesController : ControllerBase
    {
        private readonly IMesProxy _meses;

        public MesController(IMesProxy meses)
        {
            _meses = meses;
        }

        [HttpGet]

        public async Task<List<MesDto>> GetAll()
        {
            var result = await _meses.GetAllMesesAsync();

            return result;
        }


        [HttpGet("getMes/{mes}")]
        public async Task<MesDto> GetMes(int mes)
        {
            var result = await _meses.GetMesByIdAsync(mes);

            return result;
        }
    }
}
