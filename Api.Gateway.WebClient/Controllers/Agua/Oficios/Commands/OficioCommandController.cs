using Api.Gateway.Models.Oficios.Commands;
using Api.Gateway.Proxies.Agua.Oficios.Commands;
using Api.Gateway.Proxies.Agua.Oficios.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.Oficios.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/oficios")]
    public class OficioCommandController : ControllerBase
    {
        private readonly ICOficioAguaProxy _oficios;

        public OficioCommandController(ICOficioAguaProxy oficios)
        {
            _oficios = oficios;
        }

        [Consumes("multipart/form-data")]
        [Route("createOficio")]
        [HttpPost]
        public async Task<IActionResult> createOficio([FromForm] OficioCreateCommand request)
        {
            var oficio = await _oficios.CreateOficio(request);

            return Ok(oficio);
        }

        [Route("createDetalleOficio")]
        [HttpPost]
        public async Task<IActionResult> CreateDetalleOficio([FromBody] List<DetalleOficioCreateCommand> request)
        {
            var oficio = await _oficios.CreateDetalleOficio(request);
            return Ok(oficio);
        }

        [Route("deleteDetalleOficio")]
        [HttpPost]
        public async Task<IActionResult> DeleteDetalleOficio([FromBody] DetalleOficioDeleteCommand request)
        {
            var oficio = await _oficios.DeleteDetalleOficio(request);
            return Ok(oficio);
        }

        [Route("corregirOficio")]
        [HttpPost]
        public async Task<IActionResult> CorregirOficio([FromBody] CorregirOficioCommand request)
        {
            var oficio = await _oficios.CorregirOficio(request);
            return Ok(oficio);
        }

        [Route("eDGPPTOficio")]
        [HttpPost]
        public async Task<IActionResult> EDGPPTOficio([FromBody] EDGPPTOficioCommand request)
        {
            var oficio = await _oficios.EDGPPTOficio(request);
            return Ok(oficio);
        }

        [Route("pagarOficio")]
        [HttpPost]
        public async Task<IActionResult> PagarOficio([FromBody] PagarOficioCommand request)
        {
            var oficio = await _oficios.PagarOficio(request);
            return Ok(oficio);
        }
    }
}
