using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using Api.Gateway.Proxies.Transporte.EntregablesContrato.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Transporte.EntregablesContrato.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("transporte/entregablesContrato")]
    public class EContratoCommandController : ControllerBase
    {
        private readonly ICEContratoTransporteProxy _entregables;

        public EContratoCommandController(ICEContratoTransporteProxy entregables)
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
