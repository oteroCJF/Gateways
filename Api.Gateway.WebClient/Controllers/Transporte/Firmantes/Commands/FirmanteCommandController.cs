using Api.Gateway.Models.Firmantes.Commands;
using Api.Gateway.Models.Firmantes.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Transporte.Firmantes.Commands;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Transporte.Firmantes.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("transporte/firmantes")]
    public class FirmanteCommandController : ControllerBase
    {
        private readonly ICFirmanteTransporteProxy _firmantes;

        public FirmanteCommandController(ICFirmanteTransporteProxy firmantes, IUsuarioProxy usuarios, IInmuebleProxy inmuebles)
        {
            _firmantes = firmantes;
        }


        [HttpPost]
        [Route("createFirmantes")]
        public async Task<IActionResult> CreateFirmantes([FromBody] FirmanteCreateCommand firmantes)
        {
            var firmante = await _firmantes.CreateFirmantes(firmantes);
            return Ok(firmante);
        }

        [HttpPut]
        [Route("updateFirmantes")]
        public async Task<IActionResult> UpdateFirmantes([FromBody] FirmanteUpdateCommand firmantes)
        {
            var firmante = await _firmantes.UpdateFirmantes(firmantes);
            return Ok(firmante);
        }
    }

}
