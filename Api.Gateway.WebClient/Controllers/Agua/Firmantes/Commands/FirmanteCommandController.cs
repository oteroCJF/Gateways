using Api.Gateway.Models.Firmantes.Commands;
using Api.Gateway.Models.Firmantes.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Agua.Firmantes.Commands;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.Firmantes.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/firmantes")]
    public class FirmanteCommandController : ControllerBase
    {
        private readonly ICFirmanteAguaProxy _firmantes;

        public FirmanteCommandController(ICFirmanteAguaProxy firmantes, IUsuarioProxy usuarios, IInmuebleProxy inmuebles)
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
