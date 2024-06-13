using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using Api.Gateway.Proxies.Mensajeria.EntregablesContrato.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.EntregablesContrato.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("mensajeria/entregablesContrato")]
    public class EContratoCommandController : ControllerBase
    {
        private readonly ICEContratoMensajeriaProxy _entregables;

        public EContratoCommandController(ICEContratoMensajeriaProxy entregables)
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
