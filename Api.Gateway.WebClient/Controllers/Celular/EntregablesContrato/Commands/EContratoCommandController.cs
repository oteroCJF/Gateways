using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using Api.Gateway.Proxies.Celular.EntregablesContrato.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Celular.EntregablesContrato.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("celular/entregablesContrato")]
    public class EContratoCommandController : ControllerBase
    {
        private readonly ICEContratoCelularProxy _entregables;

        public EContratoCommandController(ICEContratoCelularProxy entregables)
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
