using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Convencional.ServiciosContrato.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Convencional.ServiciosContratos.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("convencional/servicioContrato")]
    public class SContratoQueryController : ControllerBase
    {
        private readonly IQSContratoConvencionalProxy _scontrato;


        public SContratoQueryController(IQSContratoConvencionalProxy scontrato)
        {
            _scontrato = scontrato;
        }

        [Route("getServiciosContrato/{contrato}")]
        [HttpGet]
        public async Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato)
        {
            return await _scontrato.GetServiciosByContrato(contrato);
        }
    }
}
