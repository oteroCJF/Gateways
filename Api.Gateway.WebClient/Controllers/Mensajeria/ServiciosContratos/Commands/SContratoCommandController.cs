using Api.Gateway.Models.Contratos.Commands.ServicioContrato;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Mensajeria.ServiciosContrato.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.ServiciosContratos.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/servicioContrato")]
    public class SContratoCommandController : ControllerBase
    {
        private readonly ICSContratoMensajeriaProxy _scontrato;


        public SContratoCommandController(ICSContratoMensajeriaProxy scontrato)
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
