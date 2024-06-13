using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Proxies.Comedor.Respuestas.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.Respuestas.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("comedor/respuestas")]
    public class RespuestaCommandController : ControllerBase
    {
        private readonly ICRespuestaComedorProxy _respuestas;

        public RespuestaCommandController(ICRespuestaComedorProxy respuestas)
        {
            _respuestas = respuestas;
        }


        [Route("updateRespuestasByCedula")]
        [HttpPut]
        public async Task<IActionResult> UpdateRespuestas([FromBody] List<RespuestasUpdateCommand> respuestas)
        {
            await _respuestas.UpdateRespuestas(respuestas);
            return Ok();
        }
    }
}
