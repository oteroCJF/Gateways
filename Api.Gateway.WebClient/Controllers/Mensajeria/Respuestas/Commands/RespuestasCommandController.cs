using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Proxies.Mensajeria.Respuestas.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Respuestas.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/respuestas")]
    public class RespuestaCommandController : ControllerBase
    {

        private readonly ICRespuestaMensajeriaProxy _respuestas;

        public RespuestaCommandController(ICRespuestaMensajeriaProxy respuestas)
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
