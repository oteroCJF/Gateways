using Api.Gateway.Models.Contratos.Commands.ServicioContrato;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Limpieza.ServicioContrato;
using Api.Gateway.Proxies.Microbiologicos.ServiciosContrato.Commands;
using Api.Gateway.Proxies.Microbiologicos.ServiciosContrato.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Microbiologicos.ServiciosContratos.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("microbiologicos/servicioContrato")]
    public class SContratoQueryController : ControllerBase
    {
        private readonly IQSContratoMicrobiologicosProxy _scontrato;


        public SContratoQueryController(IQSContratoMicrobiologicosProxy scontrato)
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
