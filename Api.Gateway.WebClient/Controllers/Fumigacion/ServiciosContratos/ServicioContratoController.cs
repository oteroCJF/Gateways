using Api.Gateway.Models.Contratos.Commands.ServicioContrato;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Fumigacion.ServicioContrato;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Fumigacion.ServiciosContratos
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("fumigacion/servicioContrato")]
    public class ServicioContratoController : ControllerBase
    {
        private readonly IFServicioContratoProxy _scontrato;


        public ServicioContratoController(IFServicioContratoProxy scontrato)
        {
            _scontrato = scontrato;
        }

        [Route("getServiciosContrato/{contrato}")]
        [HttpGet]
        public async Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato)
        {
            return await _scontrato.GetServiciosByContrato(contrato);
        }

        [Route("createSContrato")]
        [HttpPost]
        public async Task<IActionResult> CreateContrato([FromBody] ServicioContratoCreateCommand contrato)
        {
            var success = await _scontrato.CreateServicioContrato(contrato);
            return Ok(success);
        }

        [Route("updateSContrato")]
        [HttpPut]
        public async Task<IActionResult> UpdateContrato([FromBody] ServicioContratoUpdateCommand contrato)
        {
            var success = await _scontrato.UpdateServicioContrato(contrato);
            return Ok(success);
        }

        [Route("deleteSContrato")]
        [HttpPut]
        public async Task<IActionResult> DeleteContrato([FromBody] ServicioContratoDeleteCommand contrato)
        {
            int success = await _scontrato.DeleteServicioContrato(contrato);
            return Ok(success);
        }
    }
}
