using Api.Gateway.Models.Contratos.Commands.ServicioContrato;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Comedor.ServiciosContrato.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.ServiciosContratos.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/servicioContrato")]
    public class SContratoCommandController : ControllerBase
    {
        private readonly ICSContratoComedorProxy _scontrato;


        public SContratoCommandController(ICSContratoComedorProxy scontrato)
        {
            _scontrato = scontrato;
        }

        [Route("createSContrato")]
        [HttpPost]
        public async Task<IActionResult> CreateContrato([FromBody] ServicioContratoCreateCommand contrato)
        {
            var success = await _scontrato.CreateServicioContrato(contrato);
            return Ok(success);
        }

        [Route("updateSContrato")]
        [HttpPut]
        public async Task<IActionResult> UpdateContrato([FromBody] ServicioContratoUpdateCommand contrato)
        {
            var success = await _scontrato.UpdateServicioContrato(contrato);
            return Ok(success);
        }

        [Route("deleteSContrato")]
        [HttpPut]
        public async Task<IActionResult> DeleteContrato([FromBody] ServicioContratoDeleteCommand contrato)
        {
            int success = await _scontrato.DeleteServicioContrato(contrato);
            return Ok(success);
        }
    }
}
