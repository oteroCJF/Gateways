using Api.Gateway.Proxies.BMuebles.Solicitudes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.BMuebles.Solicitudes.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("bmuebles/solicitudes")]
    public class SolicitudQueryController : ControllerBase
    {
        private readonly IBMSolicitudProxy _solicitudes;

        public SolicitudQueryController(IBMSolicitudProxy solicitudes)
        {
            _solicitudes = solicitudes;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSolicitudes()
        {
            var solicitudes = await _solicitudes.GetAllSolicitudes();
            return Ok(solicitudes);
        }

        [HttpGet]
        [Route("getSolicitudByid/{id}")]
        public async Task<IActionResult> GetSolicitudById(int id)
        {
            var solicitud = await _solicitudes.GetSolicitudById(id);
            return Ok(solicitud);
        }
    }
}
