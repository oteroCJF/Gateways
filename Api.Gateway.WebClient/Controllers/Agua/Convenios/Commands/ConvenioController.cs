using Api.Gateway.Models.Convenios.Commands;
using Api.Gateway.Proxies.Agua.Convenios.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.Convenios.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/convenios")]
    public class ConvenioController : ControllerBase
    {
        private readonly ICConvenioAguaProxy _convenios;

        public ConvenioController(ICConvenioAguaProxy convenios)
        {
            _convenios = convenios;
        }

        [Route("createConvenio")]
        [HttpPost]
        public async Task<IActionResult> CreateConvenio([FromBody] ConvenioCreateCommand contrato)
        {
            int success = await _convenios.CreateConvenio(contrato);
            return Ok(success);
        }

        [Route("updateConvenio")]
        [HttpPut]
        public async Task<IActionResult> UpdateConvenio([FromBody] ConvenioUpdateCommand contrato)
        {
            int success = await _convenios.UpdateConvenio(contrato);
            return Ok(success);
        }

        [Route("deleteConvenio")]
        [HttpPut]
        public async Task<IActionResult> DeleteConvenio([FromBody] ConvenioDeleteCommand contrato)
        {
            int success = await _convenios.DeleteConvenio(contrato);
            return Ok(success);
        }
    }
}
