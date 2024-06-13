using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using Api.Gateway.Proxies.Comedor.EntregablesContrato.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.EntregablesContrato.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("comedor/entregablesContrato")]
    public class EContratoCommandController : ControllerBase
    {
        private readonly ICEContratoComedorProxy _entregables;

        public EContratoCommandController(ICEContratoComedorProxy entregables)
        {
            _entregables = entregables;
        }

        [Consumes("multipart/form-data")]
        [Route("updateEntregableContratacion")]
        [HttpPut]
        public async Task<IActionResult> UpdateEntregable([FromForm] EntregableContratoUpdateCommand entregable)
        {
            entregable.Convenio = entregable.Convenio == null ? "" : entregable.Convenio;
            int status = await _entregables.UpdateEntregable(entregable);
            return Ok(status);
        }

    }
}
