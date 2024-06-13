using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.Proxies.Comedor.Contratos.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.Contratos.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/contratos")]
    public class ContratoComedorController : ControllerBase
    {
        private readonly ICContratoComedorProxy _contratos;

        public ContratoComedorController(ICContratoComedorProxy contratos)
        {
            _contratos = contratos; 
        }

        [Route("createContrato")]
        [HttpPost]
        public async Task<IActionResult> CreateContrato([FromBody] ContratoCreateCommand contrato)
        {
            int success = await _contratos.CreateContrato(contrato);
            return Ok(success);
        }

        [Route("updateContrato")]
        [HttpPut]
        public async Task<IActionResult> UpdateContrato([FromBody] ContratoUpdateCommand contrato)
        {
            int success = await _contratos.UpdateContrato(contrato);
            return Ok(success);
        }

        [Route("deleteContrato")]
        [HttpPut]
        public async Task<IActionResult> DeleteContrato([FromBody] ContratoDeleteCommand contrato)
        {
            int success = await _contratos.DeleteContrato(contrato);
            return Ok(success);
        }
    }
}
