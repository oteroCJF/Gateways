using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas;
using Api.Gateway.Proxies.Agua.Respuestas.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.Respuestas.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/respuestas")]
    public class RespuestaCommandController : ControllerBase
    {

        private readonly ICRespuestaAguaProxy _respuestas;

        public RespuestaCommandController(ICRespuestaAguaProxy respuestas)
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
