using Api.Gateway.Models.BMuebles.Solicitudes.Commands;
using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.Proxies.BMuebles.Solicitudes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.BMuebles.Solicitudes.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("bmuebles/solicitudes")]
    public class SolicitudCommandController : ControllerBase
    {
        private readonly IBMSolicitudProxy _solicitudes;

        public SolicitudCommandController(IBMSolicitudProxy solicitudes)
        {
            _solicitudes = solicitudes;
        }

        [Route("createSolicitud")]
        [HttpPost]
        public async Task<IActionResult> CreateSolicitud([FromBody] SolicitudCreateCommand solicitud)
        {
            var success = await _solicitudes.CreateSolicitud(solicitud);
            return Ok(success);
        }
    }
}
